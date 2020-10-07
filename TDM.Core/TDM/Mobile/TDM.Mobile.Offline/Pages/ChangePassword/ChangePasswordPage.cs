using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.CRUDDetail;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.Login;
using TDM.Mobile.AppCore;
using TDM.Mobile.Supermodel.Persistence;

namespace TDM.Mobile.Pages.ChangePassword
{
    public class ChangePasswordPage : CRUDDetailPage<TDMUserUpdatePassword, TDMUserUpdatePasswordXFModel, TDMWebApiDataContext>
    {
        #region Constructors
        public ChangePasswordPage()
        {
            TDMApp.RunningApp.ChangePasswordPage = this;
        }
        #endregion

        #region Overrides
        protected override async Task SaveItemInternalAsync(TDMUserUpdatePassword model)
        {
            if (!string.IsNullOrEmpty(model.OldPassword) || !string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmPassword))
            {
                await using (TDMApp.RunningApp.NewUnitOfWork<TDMWebApiDataContext>())
                {
                    model.Update();
                    //await UnitOfWorkContext.FinalSaveChangesAsync();
                    var basicAuthHeader = (BasicAuthHeaderGenerator)FormsApplication<TDMApp>.RunningApp.AuthHeaderGenerator;
                    basicAuthHeader.Password = model.NewPassword;
                    await basicAuthHeader.SaveToAppPropertiesAsync();
                }
            }
        }
        #endregion
    }
}
