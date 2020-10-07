#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;
using Supermodel.Presentation.WebMonk.Extensions.Gateway;
using WebMonk.Context;
using WebMonk.Filters;
using WebMonk.Results;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;

namespace WMWeb.Mvc.ToDoListPage
{
    [Authorize]
    public class ToDoListMvcController : EnhancedCRUDMvcController<ToDoList, ToDoListMvcModel, ToDoListSearchMvcModel, ToDoListMvcView, DataContext>
    {
        #region Overrdies
        protected override IQueryable<ToDoList> GetItems()
        {
            var currentUserId = UserHelper.GetCurrentUserId();
            return base.GetItems().Where(x => x.ListOwnerId == currentUserId);
        }
        protected override Task<ActionResult> AfterCreateAsync(long id, ToDoList entityItem, ToDoListMvcModel mvcModelItem)
        {
            var listOwnerId = UserHelper.GetCurrentUserId();
            if (listOwnerId == null) throw new UnauthorizedAccessException();
            entityItem.ListOwnerId = listOwnerId.Value;
            HttpContext.Current.TempData.Super().NextPageModalMessage = "To Do List Created Successfully!";
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
