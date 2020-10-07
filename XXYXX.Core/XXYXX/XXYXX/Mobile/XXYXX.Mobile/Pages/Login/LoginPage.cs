#nullable enable

using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.Login;
using Xamarin.Forms;
using XXYXX.Mobile.AppCore;
using XXYXX.Mobile.Pages.Home;
using XXYXX.Mobile.Supermodel.Persistence;

namespace XXYXX.Mobile.Pages.Login
{
    public class LoginPage : UsernameAndPasswordLoginPage<XXYXXUserUpdatePassword, XXYXXWebApiDataContext>
    {
        #region Constructors
        public LoginPage()
        {
            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                #pragma warning disable 618
                Source = ImageSource.FromResource("XXYXX.Mobile.EmbeddedResources.Logo.png"),
                #pragma warning restore 618
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            LoginView.ContentView.SetUpLoginImage(logoImage);
        }
        #endregion

        #region Overrdies
        public override async Task<bool> OnSuccessfulLoginAsync(bool autoLogin, bool isJumpBack)
        {
            var homePage = XXYXXApp.RunningApp.HomePage = new HomePage();
            await Navigation.PushAsync(homePage);
            return true;
        }
        public override IAuthHeaderGenerator GetAuthHeaderGenerator(UsernameAndPasswordLoginViewModel loginViewModel)
        {
            return new BasicAuthHeaderGenerator(loginViewModel.Username, loginViewModel.Password, XXYXXApp.RunningApp.LocalStorageEncryptionKey);
        }
        #endregion
    }
}
