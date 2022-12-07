#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Controllers.Mvc;
using Supermodel.Presentation.Mvc.Extensions.Gateway;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class InlineToDoListController : EnhancedCRUDController<ToDoList, ToDoListMvcModel, ToDoListSearchMvcModel, DataContext>
    {
        #region Overrdies
        protected override IQueryable<ToDoList> GetItems()
        {
            var currentUserId = UserHelper.GetCurrentUserId();
            return base.GetItems().Where(x => x.ListOwnerId == currentUserId);
        }
        protected override Task<IActionResult> AfterCreateAsync(long id, ToDoList entityItem, ToDoListMvcModel mvcModelItem)
        {
            var listOwnerId = UserHelper.GetCurrentUserId();
            if (listOwnerId == null) throw new UnauthorizedAccessException();
            entityItem.ListOwnerId = listOwnerId.Value;
            TempData.Super().NextPageModalMessage = "To Do List Created Successfully!";
            return Task.FromResult(StayOnDetailScreen(entityItem.Id));
        }
        protected override IQueryable<ToDoList> ApplySearchBy(IQueryable<ToDoList> items, ToDoListSearchMvcModel searchBy)
        {
            items = base.ApplySearchBy(items, searchBy);
            var searchTerm = searchBy.SearchTerm.Value;
            if (!string.IsNullOrEmpty(searchTerm)) items = items.Where(l => l.Name.Contains(searchTerm) || l.ToDoItems.Any(i => i.Name.Contains(searchTerm)));
            return items;
        }
        #endregion
    }
}
