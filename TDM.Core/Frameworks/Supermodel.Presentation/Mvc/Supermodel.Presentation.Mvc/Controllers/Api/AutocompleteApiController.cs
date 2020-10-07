#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.UnitOfWork;

namespace Supermodel.Presentation.Mvc.Controllers.Api
{
    [ApiController, Route("[controller]")]
    public abstract class AutocompleteApiController<TEntity, TDataContext>  : ControllerBase 
        where TDataContext : class, IDataContext, new()
        where TEntity : class, IEntity, new()
    {
        #region ActionMethods
        public virtual async Task<IActionResult> Get(string term)
        {
            await using (new UnitOfWorkIfNoAmbientContext<TDataContext>(MustBeWritable.No))
            {
                var items = GetItems();
                var output = await AutocompleteAsync(items, term);
                return StatusCode((int)HttpStatusCode.OK, output);
            }
        }
        #endregion

        #region Protected Helpers
        protected abstract Task<List<string>> AutocompleteAsync(IQueryable<TEntity> items, string term);
        protected virtual IQueryable<TEntity> GetItems()
        {
            return ControllerCommon.GetItems<TEntity>();
        }
        #endregion
    }
}
