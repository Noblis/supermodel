#nullable enable

using System.Linq;
using Microsoft.AspNetCore.Authorization;
using MvcCoreTester.DataContexts;
using MvcCoreTester.Models;
using Supermodel.Presentation.Mvc.Controllers.Api;

namespace MvcCoreTester.ApiControllers
{
    [Authorize]
    [Authorize(Roles = "Adder")]
    public class StudentApiController : EnhancedCRUDApiController<Student, StudentApiModel, StudentSearchApiModel, DataContext>
    {
        protected override IQueryable<Student> ApplySearchBy(IQueryable<Student> items, StudentSearchApiModel searchBy)
        {
            if (!string.IsNullOrEmpty(searchBy.Term)) items = items.Where(x => x.FirstName.Contains(searchBy.Term) || x.LastName.Contains(searchBy.Term));
            return items;
        }
    }
}
