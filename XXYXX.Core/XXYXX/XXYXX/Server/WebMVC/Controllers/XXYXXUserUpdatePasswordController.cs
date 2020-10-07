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
using WebMVC.Models;

namespace WebMVC.Controllers
{
    [Authorize]
    public class XXYXXUserUpdatePasswordController : CRUDController<XXYXXUser, XXYXXUserUpdatePasswordMvcModel, DataContext>
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
            var mvcModelItem = new XXYXXUserUpdatePasswordMvcModel();
            await TryUpdateModelAsync(mvcModelItem);
            var user = await UserHelper.GetCurrentUserAsync<XXYXXUser, DataContext>();
            if (user == null) throw new UnauthorizedAccessException();
            if (user.Id != id || !user.PasswordEquals(mvcModelItem.OldPassword.Value))
            {
                TempData.Super().NextPageModalMessage = "Incorrect Old Password!";
                return RedirectToAction(nameof(Detail), new { id });
            }
            return await base.Detail(user.Id, isInline, ignore);
        }

        protected override Task<IActionResult> AfterUpdateAsync(long id, XXYXXUser entityItem, XXYXXUserUpdatePasswordMvcModel mvcModelItem)
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