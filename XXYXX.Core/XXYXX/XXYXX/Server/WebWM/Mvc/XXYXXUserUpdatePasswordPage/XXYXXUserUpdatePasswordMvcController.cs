#nullable enable

using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;
using Supermodel.Presentation.WebMonk.Extensions.Gateway;
using WebMonk.Context;
using WebMonk.Filters;
using WebMonk.Results;

namespace WebWM.Mvc.XXYXXUserUpdatePasswordPage
{
    [Authorize]
    public class XXYXXUserUpdatePasswordMvcController : CRUDMvcController<XXYXXUser, XXYXXUserUpdatePasswordMvcModel, XXYXXUserUpdatePasswordMvcView, DataContext>
    {
        #region Overrides
        public override Task<ActionResult> GetDetailAsync(long id)
        {
            var userId = UserHelper.GetCurrentUserId();
            if (userId == null) throw new UnauthorizedAccessException();
            return base.GetDetailAsync(id);
        }
        public override async Task<ActionResult> PutDetailAsync(long id, bool? isInline = null)
        {
            var mvcModelItem = new XXYXXUserUpdatePasswordMvcModel();
            await TryUpdateModelAsync(mvcModelItem);

            //because we are in Authorize header, current user should not be null
            var currentUser = await UserHelper.GetCurrentUserAsync<XXYXXUser, DataContext>();
            if (currentUser == null) throw new Exception("currentUser == null: This should never happen");
            if (currentUser.Id != id || !currentUser.PasswordEquals(mvcModelItem.OldPassword.Value))
            {
                HttpContext.Current.TempData.Super().NextPageModalMessage = "Incorrect Old Password!";
                return RedirectToAction<XXYXXUserUpdatePasswordMvcController>(x => x.GetDetailAsync(id));
            }
            return await base.PutDetailAsync(currentUser.Id, isInline);
        }

        protected override Task<ActionResult> AfterUpdateAsync(long id, XXYXXUser entityItem, XXYXXUserUpdatePasswordMvcModel mvcModelItem)
        {
            HttpContext.Current.TempData.Super().NextPageModalMessage = "Password Updated Successfully!";
            return Task.FromResult((ActionResult)RedirectToAction<XXYXXUserUpdatePasswordMvcController>(x => x.GetDetailAsync(id)));
        }

        #endregion
        
        #region Disabled Action Methods
        public override Task<ActionResult> GetListAsync() { throw new InvalidOperationException(); }
        public override Task<ActionResult> DeleteDetailAsync(long id) { throw new InvalidOperationException(); }
        public override Task<ActionResult> PostDetailAsync(long id, bool? isInline = null) { throw new InvalidOperationException(); }
        public override Task<ActionResult> DeleteBinaryFileAsync(long id, string pn) { throw new InvalidOperationException(); }
        public override Task<ActionResult> GetBinaryFileAsync(long id, string pn) { throw new InvalidOperationException(); }
        #endregion
    }
}
