#nullable enable

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Controllers.Api;
using Supermodel.Presentation.WebMonk.Models.Api;
using WebMonk.Filters;
using WebMonk.Results;

namespace WebWM.Api.XXYXXUserUpdatePasswordApi
{
    [Authorize]
    public class XXYXXUserUpdatePasswordApiController : CRUDApiController<XXYXXUser, XXYXXUserUpdatePasswordApiModel, DataContext> 
    {
        #region Action Methods
        public override async Task<ActionResult> PutAsync(long id, XXYXXUserUpdatePasswordApiModel apiModelItem)
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
                return new JsonApiResult(validationErrors, HttpStatusCode.ExpectationFailed);
            }
            return await base.PutAsync(id, apiModelItem);        
        }
        #endregion

        #region Disabled Action Methods (we just need put and validate login)
        public override Task<ActionResult> GetAllAsync(int? smSkip = null, int? smTake = null, string? smSortBy = null) { throw new InvalidOperationException(); }
        public override Task<ActionResult> GetCountAllAsync(int? smSkip = null, int? smTake = null) { throw new InvalidOperationException(); }
        public override Task<ActionResult> GetAsync(long id) { throw new InvalidOperationException(); }
        public override Task<ActionResult> DeleteAsync(long id) { throw new InvalidOperationException(); }
        public override Task<ActionResult> PostAsync(XXYXXUserUpdatePasswordApiModel apiModelItem) { throw new InvalidOperationException(); }
        #endregion
    }
}
