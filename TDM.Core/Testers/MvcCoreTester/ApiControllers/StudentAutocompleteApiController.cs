#nullable enable

using System;
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
        #region Overrides
        protected override async Task<List<Student>> AutocompleteAsync(IQueryable<Student> items, string text)
        {
            return await items
                .Where(x => x.FirstName.Contains(text) || x.LastName.Contains(text))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(3)
                .ToListAsync();
        }

        public override string GetStringFromEntity(Student entity) =>  $"{entity.FirstName} {entity.LastName}";

        //This is used in search. For search we don't need to map to entity
        public override Task<Student?> GetEntityFromNameAsync(string uniqueName) => throw new InvalidOperationException(); 
        #endregion
    }

    [Authorize]
    public class Student2AutocompleteApiController : AutocompleteApiController<Student, DataContext>
    {
        #region Overrides
        protected override async Task<List<Student>> AutocompleteAsync(IQueryable<Student> items, string text)
        {
            return await items
                .Where(x => x.FirstName.Contains(text) || x.LastName.Contains(text))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(3)
                .ToListAsync();
        }

        public override string GetStringFromEntity(Student entity) =>  $"{entity.FirstName} {entity.LastName}";

        //This is used in search. For search we don't need to map to entity
        public override Task<Student?> GetEntityFromNameAsync(string uniqueName) => throw new InvalidOperationException(); 
        #endregion
    }

}