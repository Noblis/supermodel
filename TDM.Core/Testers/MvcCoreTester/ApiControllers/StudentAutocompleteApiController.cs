#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MvcCoreTester.DataContexts;
using MvcCoreTester.Models;
using Supermodel.Presentation.Mvc.Controllers.Api;

namespace MvcCoreTester.ApiControllers
{
    [Authorize]
    public class StudentAutocompleteApiController : AutocompleteApiController<Student, DataContext>
    {
        protected override async Task<List<string>> AutocompleteAsync(IQueryable<Student> items, string text)
        {
            return await items
                .Where(x => x.FirstName.Contains(text) || x.LastName.Contains(text))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(3)
                .Select(x => $"{x.FirstName} {x.LastName}").ToListAsync();
        }
    }

    [Authorize]
    public class Student2AutocompleteApiController : AutocompleteApiController<Student, DataContext>
    {
        protected override async Task<List<string>> AutocompleteAsync(IQueryable<Student> items, string text)
        {
            return await items
                .Where(x => x.FirstName.Contains(text) || x.LastName.Contains(text))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(3)
                .Select(x => $"{x.LastName}, {x.FirstName}").ToListAsync();
        }
    }

}