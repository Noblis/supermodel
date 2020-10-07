#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Mvc.Controllers.Api;
using Z.EntityFramework.Plus;

namespace Web.ApiControllers
{
    public class DeleteAllBeforeInput
    {
        public DateTime OlderThanUtc { get; set; }
    }

    public class DeleteAllBeforeOutput
    {
        public long DeletedCount { get; set; }
    }
    
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
