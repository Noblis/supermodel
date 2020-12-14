#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pluralize.NET.Core;
using Supermodel.DataAnnotations;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Models;
using Supermodel.Presentation.Cmd.Rendering;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.Cmd.Controllers
{
    public class CRUDCmdController<TEntity, TCmdModel, TDataContext> : CRUDCmdController<TEntity, TCmdModel, TCmdModel, TDataContext>
        where TEntity : class, IEntity, new()
        where TCmdModel : CmdModelForEntity<TEntity>, new()
        where TDataContext : class, IDataContext, new()
    { 
        #region Constructors
        public CRUDCmdController(string detailTitle, string? listTitle = null) : base(detailTitle, listTitle) { }
        #endregion
    }

    public class CRUDCmdController<TEntity, TDetailMvcModel, TListMvcModel, TDataContext>
        where TEntity : class, IEntity, new()
        where TDetailMvcModel : CmdModelForEntity<TEntity>, new()
        where TListMvcModel : CmdModelForEntity<TEntity>, new()
        where TDataContext : class, IDataContext, new()
    {
        #region Constructors
        public CRUDCmdController(string detailTitle, string? listTitle = null)
        { 
            DetailTitle = detailTitle;
            ListTitle = listTitle ?? new Pluralizer().Pluralize(DetailTitle);
        }
        #endregion

        #region Action Methods
        public virtual async Task RunCRUDAsync()
        {
            var currentColors = FBColors.FromCurrent();

            while(true)
            {
                await ListAsync();
                
                //method returns true if we quit
                if (await TryShowPromptAndProcessActionAsync()) break; 
            }

            currentColors.SetColors();
        }
        public virtual async Task ListAsync()
        {
            if (ClearScreenOnList) Console.Clear();

            await using (new UnitOfWork<TDataContext>(ReadOnly.Yes))
            {
                var entities = await GetItems().ToListAsync().ConfigureAwait(false);
                var mvcModels = new List<TListMvcModel>();
                mvcModels = await mvcModels.MapFromAsync(entities).ConfigureAwait(false);

                //Init mvc model if it requires async initialization
                foreach (var mvcModelItem in mvcModels)
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    if (mvcModelItem is IAsyncInit iAsyncInit && !iAsyncInit.AsyncInitialized) await iAsyncInit.InitAsync().ConfigureAwait(false);
                }

                ShowListTitle();
                foreach (var mvcModel in mvcModels)
                {
                    CmdScaffoldingSettings.ListEntityId?.SetColors();
                    Console.Write($"{mvcModel.Id}: "); 
                    CmdScaffoldingSettings.DefaultListLabel?.SetColors();
                    mvcModel.Label.WriteLineToConsole();
                }
            }
        }
        public virtual async Task ViewDetailAsync(long id)
        {
            var mvcModelItem = await CreateMvcModelAsync(id);
            ShowViewDetailTitle(id);
            CmdRender.DisplayForModel(mvcModelItem);
            PressAnyKey();
        }
        public virtual async Task EditDetailAsync(long id)
        {
            ShowEditDetailTitle(id);
            var mvcModelItem = await CreateMvcModelAsync(id);

            while(true)
            {
                await using (new UnitOfWork<TDataContext>())
                {
                    try
                    {
                        await EditMvcModelAsync(mvcModelItem).ConfigureAwait(false);
                        if (ClearScreenOnList) PressAnyKey();
                        return;
                    }
                    catch (ModelStateInvalidException ex)
                    {
                        UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction

                        //Init ex.Model fs it requires async initialization
                        if (ex.Model is IAsyncInit iai && !iai.AsyncInitialized) await iai.InitAsync().ConfigureAwait(false);

                        CmdScaffoldingSettings.Prompt?.SetColors();
                        Console.WriteLine("Please fix the following validation errors:");
                        mvcModelItem = (TDetailMvcModel)ex.Model;
                        CmdRender.ShowValidationSummary(mvcModelItem, CmdScaffoldingSettings.ValidationErrorMessage, CmdScaffoldingSettings.InvalidValueDisplayLabel, CmdScaffoldingSettings.Prompt); //TODO: check
                    }
                }
            }
        }
        public virtual async Task AddDetailAsync()
        {
            ShowAddDetailTitle();
            var mvcModelItem = await CreateMvcModelAsync(0);

            while (true)
            {
                await using (new UnitOfWork<TDataContext>())
                {
                    try
                    {
                        await EditMvcModelAsync(mvcModelItem).ConfigureAwait(false);
                        if (ClearScreenOnList) PressAnyKey();
                        return;
                    }
                    catch (ModelStateInvalidException ex)
                    {
                        UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction

                        //Init ex.Model fs it requires async initialization
                        if (ex.Model is IAsyncInit iai && !iai.AsyncInitialized) await iai.InitAsync().ConfigureAwait(false);

                        CmdScaffoldingSettings.Prompt?.SetColors();
                        Console.WriteLine("Please fix the following validation errors:");
                        mvcModelItem = (TDetailMvcModel)ex.Model;
                        CmdRender.ShowValidationSummary(mvcModelItem, CmdScaffoldingSettings.ValidationErrorMessage, CmdScaffoldingSettings.InvalidValueDisplayLabel, CmdScaffoldingSettings.Prompt); //TODO: check
                    }
                }
            }
        }
        public virtual async Task DeleteDetailAsync(long id)
        {
            await using (new UnitOfWork<TDataContext>())
            {
                try
                {
                    var entityItem = await GetItemAsync(id).ConfigureAwait(false);
                    entityItem.Delete();
                }
                catch (UnableToDeleteException ex)
                {
                    UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction
                    CmdScaffoldingSettings.InvalidValueMessage?.SetColors();
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
                catch (Exception)
                {
                    UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction
                    CmdScaffoldingSettings.InvalidValueMessage?.SetColors();
                    
                    Console.WriteLine("ERROR: Unable to delete. Most likely reason: references from other entities.");
                }
            }
        }
        #endregion

        #region Protected Methods & Properties
        protected virtual async Task<TEntity> GetItemAndCacheItAsync(long id)
        {
            var item = await GetItemAsync(id);
            UnitOfWorkContext.CustomValues[$"Item_{id}"] = item; //we cache this, for MvcModel validation
            return item;
        }
        protected virtual Task<TEntity> GetItemAsync(long id)
        {
            return GetItems().SingleAsync(x => x.Id == id);
        }
        protected virtual IQueryable<TEntity> GetItems()
        {
            var repo = (ILinqDataRepo<TEntity>)RepoFactory.Create<TEntity>();
            return repo.Items;        }
        
        //this method return true if we quit
        protected virtual async Task<bool> TryShowPromptAndProcessActionAsync()
        {
            ShowActionPrompt();
            while (true)
            {
                CmdScaffoldingSettings.CommandValue?.SetColors();
                var input = ConsoleExt.EditLineAllCaps("", x => char.IsDigit(x) || "VEADQ".Contains(x)).Trim().ToUpper();
                if (input.StartsWith("V"))
                {
                    var id = GetIdForCommand(input);
                    if (id == null)
                    {
                        PrintInvalidCommandTryAgain();
                        continue;
                    }
                    Console.WriteLine();
                    await ViewDetailAsync(id.Value);
                    Console.WriteLine();
                    return false;
                }
                if (input.StartsWith("E"))
                {
                    var id = GetIdForCommand(input);
                    if (id == null)
                    {
                        PrintInvalidCommandTryAgain();
                        continue;
                    }
                    Console.WriteLine();
                    await EditDetailAsync(id.Value);
                    Console.WriteLine();
                    return false;
                }
                if (input == "A")
                {
                    Console.WriteLine();
                    await AddDetailAsync();
                    Console.WriteLine();
                    return false;
                }
                if (input.StartsWith("D"))
                {
                    var id = GetIdForCommand(input);
                    if (id == null)
                    {
                        PrintInvalidCommandTryAgain();
                        continue;
                    }
                    CmdScaffoldingSettings.EditLabel?.SetColors();
                    Console.Write($"Are you sure you want to delete {DetailTitle} with ID = {id}? ");
                    CmdScaffoldingSettings.EditValue?.SetColors();
                    if (!ConsoleExt.EditBool(false, CmdScaffoldingSettings.DropdownArrow, CmdScaffoldingSettings.InvalidValueMessage)) 
                    {
                        Console.WriteLine();
                        return false;
                    }

                    Console.WriteLine();
                    await DeleteDetailAsync(id.Value);
                    Console.WriteLine();
                    return false;
                }
                if (input == "Q") 
                {
                    CmdScaffoldingSettings.Prompt?.SetColors();
                    Console.WriteLine($"Quitting {ListTitle}...");
                    return true;
                }

                PrintInvalidCommandTryAgain();
            }
        }

        protected virtual long? GetIdForCommand(string input)
        {
            long? id;
            if (input.Length == 1)
            {
                CmdScaffoldingSettings.Prompt?.SetColors();
                Console.Write("Pick ID: ");
                using(CmdContext.NewRequiredScope(true, "ID"))
                {
                    id = ConsoleExt.EditInteger((long?)null) ?? throw new Exception("ID == null: this should never happen!");
                }
            }
            else
            {
                if (long.TryParse(input[1..].Trim(), out var tmpId)) id = tmpId;
                else id = null;
            }
            return id;
        }
        protected virtual void ShowActionPrompt()
        {
            CmdScaffoldingSettings.Prompt?.SetColors();
            Console.Write("Pick a command (");

            CmdScaffoldingSettings.CommandValue?.SetColors();
            Console.Write("V");

            CmdScaffoldingSettings.Prompt?.SetColors();
            Console.Write("iew, ");

            CmdScaffoldingSettings.CommandValue?.SetColors();
            Console.Write("E");

            CmdScaffoldingSettings.Prompt?.SetColors();
            Console.Write("dit, ");

            CmdScaffoldingSettings.CommandValue?.SetColors();
            Console.Write("A");

            CmdScaffoldingSettings.Prompt?.SetColors();
            Console.Write("dd, ");

            CmdScaffoldingSettings.CommandValue?.SetColors();
            Console.Write("D");

            CmdScaffoldingSettings.Prompt?.SetColors();
            // ReSharper disable once StringLiteralTypo
            Console.Write("elete, or ");

            CmdScaffoldingSettings.CommandValue?.SetColors();
            Console.Write("Q");

            CmdScaffoldingSettings.Prompt?.SetColors();
            Console.Write("uit): ");
        }
        protected virtual void PrintInvalidCommandTryAgain()
        {
            CmdScaffoldingSettings.InvalidCommand?.SetColors();
            Console.Write("Invalid command. ");

            CmdScaffoldingSettings.Prompt?.SetColors();
            Console.Write("Pick again: ");
        }

        protected virtual void ShowListTitle()
        {
            ShowTitle($"List of {ListTitle}", CmdScaffoldingSettings.Title);
        }
        protected virtual void ShowEditDetailTitle(long id)
        {
            ShowTitle($"Edit {DetailTitle} with ID = {id}", CmdScaffoldingSettings.Title);
        }
        protected virtual void ShowViewDetailTitle(long id)
        {
            ShowTitle($"View {DetailTitle} with ID = {id}", CmdScaffoldingSettings.Title);
        }
        protected virtual void ShowAddDetailTitle()
        {
            ShowTitle($"Add New {DetailTitle}", CmdScaffoldingSettings.Title);
        }
        protected virtual async Task<TDetailMvcModel> CreateMvcModelAsync(long id)
        {
            await using (new UnitOfWork<TDataContext>(ReadOnly.Yes))
            {
                var mvcModelItem = new TDetailMvcModel();

                //Init mvc model if it requires async initialization
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (mvcModelItem is IAsyncInit iAsyncInit && !iAsyncInit.AsyncInitialized) await iAsyncInit.InitAsync().ConfigureAwait(false);

                if (id == 0)
                {
                    mvcModelItem = await mvcModelItem.MapFromAsync(new TEntity()).ConfigureAwait(false);
                }
                else
                {
                    var entityItem = await GetItemAsync(id).ConfigureAwait(false);
                    mvcModelItem = await mvcModelItem.MapFromAsync(entityItem).ConfigureAwait(false);
                }

                return mvcModelItem;
            }
        }
        //this methods will catch validation exceptions that happen during mapping from mvc to domain (when it runs validation for mvc model by creating a domain object)
        protected virtual async Task<Tuple<TEntity, TDetailMvcModel>> EditMvcModelAsync(TDetailMvcModel mvcModelItem)
        {
            try
            {
                CmdRender.EditForModel(mvcModelItem);
                
                CmdContext.ValidationResultList.Clear();
                var vrl = new ValidationResultList();
                if (!await AsyncValidator.TryValidateObjectAsync(mvcModelItem, new ValidationContext(mvcModelItem), vrl).ConfigureAwait(false)) CmdContext.ValidationResultList.AddValidationResultList(vrl);
                if (CmdContext.ValidationResultList.IsValid != true) throw new ModelStateInvalidException(mvcModelItem);

                TEntity entityItem;
                if (mvcModelItem.IsNewModel()) 
                {
                    entityItem = new TEntity();
                    entityItem.Add();
                }
                else
                {
                    entityItem = await GetItemAndCacheItAsync(mvcModelItem.Id).ConfigureAwait(false);
                }
                
                entityItem = await mvcModelItem.MapToAsync(entityItem);
                if (CmdContext.ValidationResultList.IsValid != true) throw new ModelStateInvalidException(mvcModelItem);

                //Validation: we only run ValidateAsync() here because attribute - based validation is already picked up by the framework
                vrl = await mvcModelItem.ValidateAsync(new ValidationContext(mvcModelItem));
                if (vrl.Count != 0) throw new ValidationResultException(vrl);

                return Tuple.Create(entityItem, mvcModelItem);
            }
            catch (ValidationResultException ex)
            {
                CmdContext.ValidationResultList.AddValidationResultList(ex.ValidationResultList);
                throw new ModelStateInvalidException(mvcModelItem);
            }
        }
        protected virtual void PressAnyKey()
        {
            CmdScaffoldingSettings.Prompt?.SetColors();
            Console.WriteLine("Press any key...");
            while(!Console.KeyAvailable) { /* do nothing */ }
            Console.ReadKey(true);
        }
        protected virtual void ShowTitle(string title, FBColors? colors)
        {
            colors?.SetColors();
            Console.WriteLine(title);
            Console.WriteLine("".PadRight(title.Length).Replace(" ", "="));
        }
        #endregion

        #region Properties
        public string ListTitle { get; set; }
        public string DetailTitle { get; set; }
        public bool ClearScreenOnList { get; set; } = true;
        #endregion
    }
}
