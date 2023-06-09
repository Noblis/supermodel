﻿#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Controllers.Api;

namespace Web.ApiControllers
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
