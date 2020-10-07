using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.Login;
using TDM.Mobile.AppCore;
using TDM.Mobile.Pages.MyToDoListList;
using TDM.Mobile.Supermodel.Auth;
using TDM.Mobile.Supermodel.Persistence;
using Xamarin.Forms;

namespace TDM.Mobile.Pages.Login
{
    public class LoginPage : UsernameAndPasswordLoginPage<TDMUserUpdatePassword, TDMWebApiDataContext>
    {
        #region Constructors
        public LoginPage()
        {
            TDMApp.RunningApp.LoginPage = this;

            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromResource("TDM.Mobile.EmbeddedResources.Logo.png"),
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.CenterAndExpand

            };

            LoginView.ContentView.SetUpLoginImage(logoImage);
        }
        #endregion

        #region Overrdies
        public override async Task<bool> OnSuccessfulLoginAsync(bool autoLogin, bool isJumpBack)
        {
            var myToDoListListPage = FormsApplication<TDMApp>.RunningApp.MyToDoListListPage = (MyToDoListListPage) await new MyToDoListListPage().InitAsync("My Lists");
            await Navigation.PushAsync(myToDoListListPage);
            if (!isJumpBack) await myToDoListListPage.LoadListContentAsync();
            return true;
        }
        public override IAuthHeaderGenerator GetAuthHeaderGenerator(UsernameAndPasswordLoginViewModel loginViewModel)
        {
            return new TDMSecureAuthHeaderGenerator(loginViewModel.Username, loginViewModel.Password, TDMApp.RunningApp.LocalStorageEncryptionKey);
        }
        #endregion
    }
}
