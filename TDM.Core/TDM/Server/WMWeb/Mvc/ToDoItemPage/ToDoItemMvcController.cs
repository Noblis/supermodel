#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Supermodel.Persistence.Repository;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;
using WebMonk.Filters;
using WebMonk.Results;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;
using WMWeb.Mvc.ToDoListPage;

namespace WMWeb.Mvc.ToDoItemPage
{
    [Authorize]
    public class ToDoItemMvcController : ChildCRUDMvcController<ToDoItem, ToDoItemMvcModel, ToDoList, ToDoListMvcController, ToDoItemMvcView, DataContext>
    {
        #region Overrdies
        protected override IQueryable<ToDoItem> GetItems()
        {
            var currentUserId = UserHelper.GetCurrentUserId();
            return base.GetItems().Where(x => x.ParentToDoList!.ListOwnerId == currentUserId);
        }

        protected override async Task<ActionResult> AfterDeleteAsync(long id, long parentId, ToDoItem entityItem)
        {
            await UpdateParentModifiedDateAsync(parentId, entityItem);
            return await base.AfterDeleteAsync(id, parentId, entityItem);
        }
        protected override async Task<ActionResult> AfterUpdateAsync(long id, ToDoItem entityItem, ToDoItemMvcModel mvcModelItem)
        {
            await UpdateParentModifiedDateAsync(entityItem.ParentToDoListId, entityItem);
            return await base.AfterUpdateAsync(id, entityItem, mvcModelItem);
        }
        protected override async Task<ActionResult> AfterCreateAsync(long id, long parentId, ToDoItem entityItem, ToDoItemMvcModel mvcModelItem)
        {
            await UpdateParentModifiedDateAsync(entityItem.ParentToDoListId, entityItem);
            return await base.AfterCreateAsync(id, parentId, entityItem, mvcModelItem);
        }
        protected async Task UpdateParentModifiedDateAsync(long parentId, ToDoItem entityItem)
        {
            if (entityItem.ParentToDoList != null) entityItem.ParentToDoList.ModifiedOnUtc = DateTime.UtcNow;
            else (await RepoFactory.Create<ToDoList>().GetByIdAsync(parentId)).ModifiedOnUtc = DateTime.UtcNow;
        }
        #endregion
    }
}
