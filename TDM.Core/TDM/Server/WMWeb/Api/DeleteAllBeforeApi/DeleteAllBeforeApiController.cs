#nullable enable

using System.Linq;
using System.Threading.Tasks;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Controllers.Api;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;
using Z.EntityFramework.Plus;

namespace WMWeb.Api.DeleteAllBeforeApi
{
    public class DeleteAllBeforeApiController : CommandApiController<DeleteAllBeforeInput, DeleteAllBeforeOutput>
    {
        protected override async Task<DeleteAllBeforeOutput> ExecuteAsync(DeleteAllBeforeInput input)
        {
            await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var repo = (EFCoreSimpleDataRepo<ToDoList>)RepoFactory.Create<ToDoList>();
                var query = repo.Items.Where(x => x.ModifiedOnUtc <= input.OlderThanUtc);
                var count = await query.DeleteAsync();
                return new DeleteAllBeforeOutput { DeletedCount = count };
            }        
        }
    }
}
