#nullable enable

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Controllers.Api;
using Supermodel.Presentation.Mvc.Models.Api;
using WebMVC.Models;

namespace WebMVC.ApiControllers
{
    [Authorize]
    public class XXYXXUserUpdatePasswordApiController : CRUDApiController<XXYXXUser, XXYXXUserUpdatePasswordApiModel, DataContext> 
    {
        #region Action Methods
        public override async Task<IActionResult> Put(long id, XXYXXUserUpdatePasswordApiModel apiModelItem)
        {
            //Check if the old password matches
            var user = await UserHelper.GetCurrentUserAsync<XXYXXUser, DataContext>();   
            if (user == null) throw new UnauthorizedAccessException();
            if (user.Id != id  || !user.PasswordEquals(apiModelItem.OldPassword))
            {
                var error = new ValidationErrorsApiModel.Error
                {
                    Name = "OldPassword",
                    ErrorMessages = new List<string> {"Incorrect Old Password!"}
                };
                var validationErrors = new ValidationErrorsApiModel { error };
                return StatusCode((int)HttpStatusCode.ExpectationFailed, validationErrors);
            }
            return await base.Put(id, apiModelItem);        
        }
        #endregion

        #region Disabled Action Methods (we just need put and validate login)
        public override Task<IActionResult> All(int? smSkip = null, int? smTake = null, string? smSortBy = null) { throw new InvalidOperationException(); }
        public override Task<IActionResult> CountAll(int? smSkip = null, int? smTake = null) { throw new InvalidOperationException(); }
        public override Task<IActionResult> Get(long id) { throw new InvalidOperationException(); }
        public override Task<IActionResult> Delete(long id) { throw new InvalidOperationException(); }
        public override Task<IActionResult> Post(XXYXXUserUpdatePasswordApiModel apiModelItem) { throw new InvalidOperationException(); }
        #endregion
    }
}
