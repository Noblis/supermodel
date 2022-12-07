#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Controllers.Api;
using WebMonk.Filters;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;

namespace WMWeb.Api.ToDoListAutocompleteApi
{
    [Authorize]
    public class ToDoListAutocompleteApiController : AutocompleteApiController<ToDoList, DataContext>
    {
        protected override async Task<List<ToDoList>> AutocompleteAsync(IQueryable<ToDoList> items, string term)
        {
            var currentUserId = UserHelper.GetCurrentUserId();
            return await items
                .Where(x => x.ListOwnerId == currentUserId && x.Name.Contains(term))
                .ToListAsync();
        }

        public override string GetStringFromEntity(ToDoList entity) => entity.Name;

        public override Task<ToDoList?> GetEntityFromNameAsync(string uniqueName)
        {
            throw new System.NotImplementedException();
        }
    }
}
