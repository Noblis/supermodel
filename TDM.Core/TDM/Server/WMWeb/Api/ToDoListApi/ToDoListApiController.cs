#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Controllers.Api;
using WebMonk.Filters;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;

namespace WMWeb.Api.ToDoListApi
{
    [Authorize]
    public class ToDoListApiController : EnhancedCRUDApiController<ToDoList, ToDoListApiModel, SimpleSearchApiModel, DataContext>
    {
        #region Overrides
        protected override IQueryable<ToDoList> GetItems()
        {
            var currentUserId = UserHelper.GetCurrentUserId();
            if (currentUserId == null) throw new UnauthorizedAccessException();
            return base.GetItems().Where(x => x.ListOwnerId == currentUserId);        
        }
        protected override IQueryable<ToDoList> ApplySearchBy(IQueryable<ToDoList> items, SimpleSearchApiModel searchBy)
        {
            items = base.ApplySearchBy(items, searchBy);
            var searchTerm = searchBy.SearchTerm;
            if (!string.IsNullOrEmpty(searchTerm)) items = items.Where(l => l.Name.Contains(searchTerm) || l.ToDoItems.Any(i => i.Name.Contains(searchTerm)));
            return items;        
        }

        protected override Task AfterCreateAsync(ToDoList entityItem, ToDoListApiModel apiModelItem)
        {
            ValidateListOwnerId(entityItem);
            return base.AfterCreateAsync(entityItem, apiModelItem);
        }
        protected override Task AfterUpdateAsync(long id, ToDoList entityItem, ToDoListApiModel apiModelItem)
        {
            ValidateListOwnerId(entityItem);
            return base.AfterUpdateAsync(id, entityItem, apiModelItem);
        }
        protected override Task AfterDeleteAsync(long id, ToDoList entityItem)
        {
            ValidateListOwnerId(entityItem);
            return base.AfterDeleteAsync(id, entityItem);
        }
        #endregion

        #region Protected Helpers
        protected void ValidateListOwnerId(ToDoList entityItem)
        {
            if (entityItem.ListOwnerId != UserHelper.GetCurrentUserId())
            {
                UnitOfWorkContext.CommitOnDispose = false;
                throw new Exception("entityItem.ListOwnerId != UserHelper.GetCurrentUserId())");
            }
        }
        #endregion
    }
}
