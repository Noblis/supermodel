#nullable enable

using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Controllers;
using Supermodel.Presentation.Mvc.Controllers.Mvc;
using Supermodel.Presentation.Mvc.Extensions.Gateway;

namespace Web.Controllers
{
    [Authorize]
    public class TDMUserUpdatePasswordController : CRUDController<TDMUser, TDMUserUpdatePasswordMvcModel, DataContext>
    {
        #region Overrides
        public override Task<IActionResult> Detail(long id, HttpGet ignore)
        {
            var userId = UserHelper.GetCurrentUserId();
            if (userId == null) throw new UnauthorizedAccessException();
            return base.Detail(id, ignore);
        }
        public override async Task<IActionResult> Detail(long id, bool? isInline, HttpPut ignore)
        {
            var mvcModelItem = new TDMUserUpdatePasswordMvcModel();
            await TryUpdateModelAsync(mvcModelItem);

            //because we are in Authorize header, current user should not be null
            var currentUser = await UserHelper.GetCurrentUserAsync<TDMUser, DataContext>();
            if (currentUser == null) throw new Exception("currentUser == null: This should never happen");
            if (currentUser.Id != id || !currentUser.PasswordEquals(mvcModelItem.OldPassword.Value))
            {
                TempData.Super().NextPageModalMessage = "Incorrect Old Password!";
                return RedirectToAction(nameof(Detail), new { id });
            }
            return await base.Detail(currentUser.Id, isInline, ignore);
        }

        protected override Task<IActionResult> AfterUpdateAsync(long id, TDMUser entityItem, TDMUserUpdatePasswordMvcModel mvcModelItem)
        {
            TempData.Super().NextPageModalMessage = "Password Updated Successfully!";
            return Task.FromResult((IActionResult)RedirectToAction(nameof(Detail), new { id } ));
        }
        #endregion
        
        #region Disabled Action Methods
        public override Task<IActionResult> List() { throw new InvalidOperationException(); }
        public override Task<IActionResult> Detail(long id, HttpDelete ignore) { throw new InvalidOperationException(); }
        public override Task<IActionResult> Detail(long id, bool? isInline, HttpPost ignore) { throw new InvalidOperationException(); }
        public override Task<IActionResult> BinaryFile(long id, string pn, HttpDelete ignore) { throw new InvalidOperationException(); }
        public override Task<IActionResult> BinaryFile(long id, string pn, HttpGet ignore) { throw new InvalidOperationException(); }
        #endregion
    }
}
