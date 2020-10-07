#nullable enable

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Controllers.Api;
using Supermodel.Presentation.WebMonk.Models.Api;
using WebMonk.Filters;
using WebMonk.Results;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;

namespace WMWeb.Api.TDMUserUpdatePasswordApi
{
    [Authorize]
    public class TDMUserUpdatePasswordApiController : CRUDApiController<TDMUser, TDMUserUpdatePasswordApiModel, DataContext>
    {
        #region Action Methods
        public override async Task<ActionResult> PutAsync(long id, TDMUserUpdatePasswordApiModel apiModelItem)
        {
            //Check if the old password matches
            var user = await UserHelper.GetCurrentUserAsync<TDMUser, DataContext>();
            if (user == null || user.Id != id  || !user.PasswordEquals(apiModelItem.OldPassword))
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

        #region Overrides
        protected override async Task<ValidateLoginResponseApiModel> GetValidateLoginResponseAsync()
        {
            //because we are in Authorize header, current user should not be null
            var currentUser = await UserHelper.GetCurrentUserAsync<TDMUser, DataContext>();
            if (currentUser == null) throw new Exception("currentUser == null: This should never happen");
            
            return new ValidateLoginResponseApiModel { UserId = currentUser.Id, UserLabel = $"{currentUser.FirstName } {currentUser.LastName}" };
        }
        #endregion

        #region Disabled Action Methods (we just need put and validate login)
        public override Task<ActionResult> GetAllAsync(int? smSkip = null, int? smTake = null, string? smSortBy = null) { throw new InvalidOperationException(); }
        public override Task<ActionResult> GetCountAllAsync(int? smSkip = null, int? smTake = null) { throw new InvalidOperationException(); }
        public override Task<ActionResult> GetAsync(long id)  { throw new InvalidOperationException(); }
        public override Task<ActionResult> DeleteAsync(long id)  { throw new InvalidOperationException(); }
        public override Task<ActionResult> PostAsync(TDMUserUpdatePasswordApiModel apiModelItem) { throw new InvalidOperationException(); }
        #endregion
    }
}