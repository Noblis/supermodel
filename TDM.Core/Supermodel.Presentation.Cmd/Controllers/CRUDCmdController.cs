#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Supermodel.DataAnnotations;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Cmd.Models;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.Cmd.Controllers
{
    public class CRUDCmdController<TEntity, TCmdModel, TDataContext> : CRUDCmdController<TEntity, TCmdModel, TCmdModel, TDataContext>
        where TEntity : class, IEntity, new()
        where TCmdModel : CmdModelForEntity<TEntity>, new()
        where TDataContext : class, IDataContext, new()
    { 
        #region Constructors
        public CRUDCmdController(string listTitle) : base(listTitle) { }
        #endregion
    }

    public class CRUDCmdController<TEntity, TDetailMvcModel, TListMvcModel, TDataContext>
        where TEntity : class, IEntity, new()
        where TDetailMvcModel : CmdModelForEntity<TEntity>, new()
        where TListMvcModel : CmdModelForEntity<TEntity>, new()
        where TDataContext : class, IDataContext, new()
    {
        #region Constructors
        public CRUDCmdController(string listTitle)
        { 
            ListTitle = listTitle;
        }
        #endregion

        #region Action Methods
        public virtual async Task ShowListAsync()
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
        //public virtual async Task<ActionResult> GetDetailAsync(long id)
        //{
        //    var modelStateJson = (string?)HttpContext.Current.TempData[Config.ModelState];
        //    if (modelStateJson != null)
        //    {
        //        var modelState = SerializableModelState.CreateFromJson(modelStateJson);
        //        await modelState.ReplaceInContextAsync().ConfigureAwait(false);
        //    }

        //    await using (new UnitOfWork<TDataContext>(ReadOnly.Yes))
        //    {
        //        var mvcModelItem = new TDetailMvcModel();
                
        //        //Init mvc model if it requires async initialization
        //        // ReSharper disable once SuspiciousTypeConversion.Global
        //        if (mvcModelItem is IAsyncInit iAsyncInit && !iAsyncInit.AsyncInitialized) await iAsyncInit.InitAsync().ConfigureAwait(false);

        //        if (id == 0)
        //        {
        //            mvcModelItem = await mvcModelItem.MapFromAsync(new TEntity()).ConfigureAwait(false);
        //        }
        //        else
        //        {
        //            var entityItem = await GetItemAsync(id).ConfigureAwait(false);
        //            mvcModelItem = await mvcModelItem.MapFromAsync(entityItem).ConfigureAwait(false);
        //        }

        //        return new TMvcView().RenderDetail(mvcModelItem).ToHtmlResult();
        //    }
        //}
        //public virtual async Task<ActionResult> PutDetailAsync(long id, bool? isInline = null)
        //{
        //    await using (new UnitOfWork<TDataContext>())
        //    {
        //        if (id == 0) throw new SupermodelException("MvcCRUDController.Detail[Put]: id == 0");

        //        var entityItem = await GetItemAndCacheItAsync(id).ConfigureAwait(false);
        //        TDetailMvcModel mvcModelItem;
        //        try
        //        {
        //            var prefix = isInline == true ? Config.InlinePrefix : "";
        //            (entityItem, mvcModelItem) = await TryUpdateEntityAsync(entityItem, prefix).ConfigureAwait(false);
        //        }
        //        catch (ModelStateInvalidException ex)
        //        {
        //            UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction

        //            //Init ex.Model is it requires async initialization
        //            if (ex.Model is IAsyncInit iai && !iai.AsyncInitialized) await iai.InitAsync().ConfigureAwait(false);

        //            if (isInline == true)
        //            {
        //                var modelState = await SerializableModelState.CreateFromContextAsync().ConfigureAwait(false);
        //                HttpContext.Current.TempData[Config.ModelState] = modelState.SerializeToJson();
        //                return GoToListScreen(id);
        //            }
        //            else
        //            {
        //                return new TMvcView().RenderDetail((TDetailMvcModel)ex.Model).ToHtmlResult();
        //            }
        //        }

        //        return await AfterUpdateAsync(id, entityItem, mvcModelItem).ConfigureAwait(false);
        //    }
        //}
        //public virtual async Task<ActionResult> PostDetailAsync(long id, bool? isInline = null)
        //{
        //    await using (new UnitOfWork<TDataContext>())
        //    {
        //        if (id != 0) throw new SupermodelException("CRUDControllerBase.Detail[Post]: id != 0");

        //        var entityItem = new TEntity();
        //        TDetailMvcModel mvcModelItem;
        //        try
        //        {
        //            var prefix = isInline == true ? Config.InlinePrefix : "";
        //            (entityItem, mvcModelItem) = await TryUpdateEntityAsync(entityItem, prefix).ConfigureAwait(false);
        //        }
        //        catch (ModelStateInvalidException ex)
        //        {
        //            UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction

        //            //Init ex.Model fs it requires async initialization
        //            if (ex.Model is IAsyncInit iai && !iai.AsyncInitialized) await iai.InitAsync().ConfigureAwait(false);

        //            if (isInline == true)
        //            {
        //                var modelState = await SerializableModelState.CreateFromContextAsync().ConfigureAwait(false);
        //                HttpContext.Current.TempData[Config.ModelState] = modelState.SerializeToJson();
        //                return GoToListScreen(id);
        //            }
        //            else
        //            {
        //                return new TMvcView().RenderDetail((TDetailMvcModel)ex.Model).ToHtmlResult();
        //            }
        //        }
        //        entityItem.Add();

        //        return await AfterCreateAsync(id, entityItem, mvcModelItem).ConfigureAwait(false);
        //    }
        //}
        //public virtual async Task<ActionResult> DeleteDetailAsync(long id)
        //{
        //    await using (new UnitOfWork<TDataContext>())
        //    {
        //        TEntity? entityItem = null;
        //        try
        //        {
        //            entityItem = await GetItemAsync(id).ConfigureAwait(false);
        //            entityItem.Delete();
        //        }
        //        catch (UnableToDeleteException ex)
        //        {
        //            UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction
        //            HttpContext.Current.TempData.Super().NextPageModalMessage = ex.Message;
        //        }
        //        catch (Exception)
        //        {
        //            UnitOfWorkContext<TDataContext>.CurrentDataContext.CommitOnDispose = false; //rollback the transaction
        //            HttpContext.Current.TempData.Super().NextPageModalMessage = "PROBLEM!!!\\n\\nUnable to delete. Most likely reason: references from other entities.";
        //        }

        //        if (entityItem == null) throw new SupermodelException("MvcCRUDController.Detail[Delete]: entityItem == null: this should never happen");
        //        return await AfterDeleteAsync(id, entityItem).ConfigureAwait(false);
        //    }
        //}

        //public virtual async Task<ActionResult> GetBinaryFileAsync(long id, string pn)
        //{
        //    await using (new UnitOfWork<TDataContext>(ReadOnly.Yes))
        //    {
        //        var mvcModelItem = await new TDetailMvcModel().MapFromAsync(await GetItemAsync(id)).ConfigureAwait(false);

        //        var file = (BinaryFileModelBase?)mvcModelItem.PropertyGet(pn);
        //        if (file == null || file.IsEmpty) return new StatusCodeResult(HttpStatusCode.NotFound);
        //        return new BinaryFileResult(file.BinaryContent!, file.FileName!);
        //    }
        //}
        //public virtual async Task<ActionResult> DeleteBinaryFileAsync(long id, string pn)
        //{
        //    await using (new UnitOfWork<TDataContext>())
        //    {
        //        var entityItem = await GetItemAsync(id).ConfigureAwait(false);
        //        var mvcModelItem = await new TDetailMvcModel().MapFromAsync(entityItem).ConfigureAwait(false);

        //        //see if pn is a required property
        //        var propInfo = typeof(TDetailMvcModel).GetProperty(pn) ?? throw new SupermodelException("GetProperty(pn) == null");
        //        if (Attribute.GetCustomAttribute(propInfo, typeof(RequiredAttribute), true) != null)
        //        {
        //            HttpContext.Current.TempData.Super().NextPageModalMessage = "Cannot delete required field";
        //            return StayOnDetailScreen(id);
        //        }

        //        var file = (BinaryFileModelBase?)mvcModelItem.PropertyGet(pn);
        //        if (file == null) throw new SystemException("file == null");

        //        file.Empty();
        //        entityItem = await mvcModelItem.MapToAsync(entityItem).ConfigureAwait(false);

        //        var result = await AfterBinaryDeleteAsync(id, entityItem, mvcModelItem).ConfigureAwait(false);
        //        await UnitOfWorkContext<TDataContext>.CurrentDataContext.FinalSaveChangesAsync().ConfigureAwait(false);
        //        return result;
        //    }
        //}
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
        //this methods will catch validation exceptions that happen during mapping from mvc to domain (when it runs validation for mvc model by creating a domain object)
        //protected virtual async Task<Tuple<TEntity, TDetailMvcModel>> TryUpdateEntityAsync(TEntity entityItem, string prefix)
        //{
        //    var mvcModelItem = new TDetailMvcModel();
        //    // ReSharper disable once SuspiciousTypeConversion.Global
        //    if (mvcModelItem is IAsyncInit iAsyncInit && !iAsyncInit.AsyncInitialized) await iAsyncInit.InitAsync().ConfigureAwait(false);
        //    mvcModelItem = await new TDetailMvcModel().MapFromAsync(entityItem).ConfigureAwait(false);

        //    try
        //    {
        //        await TryUpdateModelAsync(mvcModelItem, prefix).ConfigureAwait(false);
        //        if (HttpContext.Current.ValidationResultList.IsValid != true) throw new ModelStateInvalidException(mvcModelItem);

        //        entityItem = await mvcModelItem.MapToAsync(entityItem).ConfigureAwait(false);
        //        if (HttpContext.Current.ValidationResultList.IsValid != true) throw new ModelStateInvalidException(mvcModelItem);

        //        //Validation: we only run ValidateAsync() here because attribute-based validation is already picked up by the framework
        //        var vrl = await mvcModelItem.ValidateAsync(new ValidationContext(mvcModelItem)).ConfigureAwait(false);
        //        if (vrl.Count != 0) throw new ValidationResultException(vrl);

        //        return Tuple.Create(entityItem, mvcModelItem);
        //    }
        //    catch (ValidationResultException ex)
        //    {
        //        HttpContext.Current.ValidationResultList.AddValidationResultList(ex.ValidationResultList, prefix);
        //        throw new ModelStateInvalidException(mvcModelItem);
        //    }
        //}
        #endregion

        #region Properties
        public string ListTitle { get; set; }
        #endregion
    }
}
