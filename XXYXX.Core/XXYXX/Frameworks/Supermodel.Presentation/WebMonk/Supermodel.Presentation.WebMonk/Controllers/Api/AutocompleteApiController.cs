#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;
using Supermodel.Persistence.UnitOfWork;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace Supermodel.Presentation.WebMonk.Controllers.Api
{
    public abstract class AutocompleteApiController<TEntity, TDataContext> : ApiController
        where TDataContext : class, IDataContext, new()
        where TEntity : class, IEntity, new()
    {
        #region ActionMethods
        public virtual async Task<ActionResult> GetAsync(string term)
        {
            await using (new UnitOfWorkIfNoAmbientContext<TDataContext>(MustBeWritable.No))
            {
                var items = GetItems();
                var output = await AutocompleteAsync(items, term).ConfigureAwait(false);
                return new JsonApiResult(output);
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
