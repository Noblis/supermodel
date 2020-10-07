#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermodel.Persistence.Repository;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Controllers.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class ToDoItemController : ChildCRUDController<ToDoItem, ToDoItemMvcModel, ToDoList, ToDoListController, DataContext>
    {
        #region Overrdies
        protected override IQueryable<ToDoItem> GetItems()
        {
            var currentUserId = UserHelper.GetCurrentUserId();
            return base.GetItems().Where(x => x.ParentToDoList!.ListOwnerId == currentUserId);
        }

        protected override async Task<IActionResult> AfterDeleteAsync(long id, long parentId, ToDoItem entityItem)
        {
            await UpdateParentModifiedDateAsync(parentId, entityItem);
            return await base.AfterDeleteAsync(id, parentId, entityItem);
        }
        protected override async Task<IActionResult> AfterUpdateAsync(long id, ToDoItem entityItem, ToDoItemMvcModel mvcModelItem)
        {
            await UpdateParentModifiedDateAsync(entityItem.ParentToDoListId, entityItem);
            return await base.AfterUpdateAsync(id, entityItem, mvcModelItem);
        }
        protected override async Task<IActionResult> AfterCreateAsync(long id, long parentId, ToDoItem entityItem, ToDoItemMvcModel mvcModelItem)
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
