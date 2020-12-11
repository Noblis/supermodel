﻿#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pluralize.NET.Core;
using Supermodel.DataAnnotations;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
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
        public virtual async Task ListAsync()
        {
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
                    CmdScaffoldingSettings.ListId?.SetColors();
                    Console.Write($"{mvcModel.Id}: "); 
                    CmdScaffoldingSettings.DefaultListLabel?.SetColors();
                    mvcModel.Label.WriteLineToConsole();
                }
            }
        }
        public virtual async Task ViewDetailAsync(long id)
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

                CmdRender.DisplayForModel(mvcModelItem);
            }
        }
        public virtual async Task EditDetailAsync(long id)
        {

        }
        public virtual async Task NewDetailAsync()
        {

        }
        public virtual async Task DeleteDetailAsync(long id)
        {
            await using (new UnitOfWork<TDataContext>())
            {
                TEntity? entityItem = null;
                try
                {
                    entityItem = await GetItemAsync(id).ConfigureAwait(false);
                    entityItem.Delete();
                }
                catch (UnableToDeleteException ex)
                {
                    UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction
                    CmdScaffoldingSettings.InvalidValueMessage?.SetColors();
                    Console.WriteLine(ex.Message);
                }
                catch (Exception)
                {
                    UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction
                    CmdScaffoldingSettings.InvalidValueMessage?.SetColors();
                    Console.WriteLine("PROBLEM: Unable to delete. Most likely reason: references from other entities.");
                }
                if (entityItem == null) throw new SupermodelException("CmdCRUDController.Detail[Delete]: entityItem == null: this should never happen");
            }
        }
        #endregion

        #region Overrides
        //protected override async Task<(bool, object?[])> TryBindAndValidateParametersAsync(MethodInfo actionMethodInfo)
        //{
        //    await using (new UnitOfWorkIfNoAmbientContext<TDataContext>(MustBeWritable.No))
        //    {
        //        return await base.TryBindAndValidateParametersAsync(actionMethodInfo).ConfigureAwait(false);
        //    }
        //}
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
        protected virtual void ShowListTitle()
        {
            CmdScaffoldingSettings.ListTitle?.SetColors();
            Console.WriteLine(ListTitle);
            CmdScaffoldingSettings.ListTitleUnderline?.SetColors();
            Console.WriteLine("".PadRight(ListTitle.Length).Replace(" ", "="));
        }
        protected virtual void ShowEditDetailTitle()
        {
            CmdScaffoldingSettings.DetailTitle?.SetColors();
            var title = $"Edit {DetailTitle}";
            Console.WriteLine(title);
            CmdScaffoldingSettings.ListTitleUnderline?.SetColors();
            Console.WriteLine("".PadRight(title.Length).Replace(" ", "="));
        }
        protected virtual void ShowNewDetailTitle()
        {
            CmdScaffoldingSettings.DetailTitle?.SetColors();
            var title = $"New {DetailTitle}";
            Console.WriteLine(title);
            CmdScaffoldingSettings.ListTitleUnderline?.SetColors();
            Console.WriteLine("".PadRight(title.Length).Replace(" ", "="));
        }
        //this methods will catch validation exceptions that happen during mapping from mvc to domain (when it runs validation for mvc model by creating a domain object)
        protected virtual async Task<Tuple<TEntity, TDetailMvcModel>> TryUpdateEntityAsync(TEntity entityItem)
        {
            var mvcModelItem = new TDetailMvcModel();
            if (mvcModelItem is IAsyncInit iAsyncInit && !iAsyncInit.AsyncInitialized) await iAsyncInit.InitAsync();
            mvcModelItem = await mvcModelItem.MapFromAsync(entityItem);

            try
            {
                CmdR
                await TryUpdateModelAsync(mvcModelItem);
                if (ModelState.IsValid != true) throw new ModelStateInvalidException(mvcModelItem);

                entityItem = await mvcModelItem.MapToAsync(entityItem);
                if (ModelState.IsValid != true) throw new ModelStateInvalidException(mvcModelItem);

                //Validation: we only run ValidateAsync() here because attribute-based validation is already picked up by the framework
                var vrl = await mvcModelItem.ValidateAsync(new ValidationContext(mvcModelItem));
                if (vrl.Count != 0) throw new ValidationResultException(vrl);

                return Tuple.Create(entityItem, mvcModelItem);
            }
            catch (ValidationResultException ex)
            {
                ModelState.AddValidationResultList(ex.ValidationResultList, prefix);
                throw new ModelStateInvalidException(mvcModelItem);
            }
        }
        #endregion

        #region Properties
        public string ListTitle { get; set; }
        public string DetailTitle { get; set; }
        #endregion
    }
}