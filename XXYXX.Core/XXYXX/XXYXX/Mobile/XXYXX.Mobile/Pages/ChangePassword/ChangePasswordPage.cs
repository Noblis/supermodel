#nullable enable

using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.UnitOfWork;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.CRUDDetail;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.Login;
using XXYXX.Mobile.AppCore;
using XXYXX.Mobile.Models;
using XXYXX.Mobile.Supermodel.Persistence;

namespace XXYXX.Mobile.Pages.ChangePassword
{
    public class ChangePasswordPage : CRUDDetailPage<XXYXXUserUpdatePassword, XXYXXUserUpdatePasswordXFModel, XXYXXWebApiDataContext>
    {
        #region Overrides
        protected override async Task SaveItemInternalAsync(XXYXXUserUpdatePassword model)
        {
            if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                await using (XXYXXApp.RunningApp.NewUnitOfWork<XXYXXWebApiDataContext>())
                {
                    model.Update();
                    await UnitOfWorkContext.FinalSaveChangesAsync();
                    var basicAuthHeader = (BasicAuthHeaderGenerator)FormsApplication<XXYXXApp>.RunningApp.AuthHeaderGenerator;
                    basicAuthHeader.Password = model.NewPassword;
                    await basicAuthHeader.SaveToAppPropertiesAsync();
                }
            }
        }
        #endregion
    }
}
