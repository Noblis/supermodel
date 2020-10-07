using System;
using System.Linq;
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

            AutoLoginIfConnectionLost = true;

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
            if (!autoLogin)
            {
                await DisplayAlert("First Sync", "Since this is your first login, you must sync before you can continue.", "Run First Sync");
                var synchronizer = new TDMSynchronizer();
                try
                {
                    LoginView.Message = "Synchronizing...";
                    await TDMApp.RunningApp.DeleteAllDataAsync();
                    await synchronizer.SynchronizeAsync();
                    await DisplayAlert("Success", "Synchronization with the cloud completed", "Ok");
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Synchronization with the cloud could not be completed", "Ok");
                    return false;
                }
            }            
            
            var myToDoListListPage = FormsApplication<TDMApp>.RunningApp.MyToDoListListPage = (MyToDoListListPage) await new MyToDoListListPage().InitAsync("My Lists");
            await Navigation.PushAsync(myToDoListListPage);
            if (!isJumpBack) await myToDoListListPage.LoadListContentAsync();
            return true;
        }
        protected override async Task<bool> OnConfirmedLogOutAsync()
        {
            var answer = true;

            var synchronizer = new TDMSynchronizer();
            if (TDMApp.RunningApp.MyToDoListListPage.Models.Any(x => synchronizer.IsUploadPending(x))) answer = await DisplayAlert("Alert", "Some of your lists have pending changes that need to be synchronized with the cloud. If you log out, you will lose your changes.", "Sign Out", "Go Back");
            
            if (answer) await TDMApp.RunningApp.DeleteAllDataAsync();
            return answer;
        }
        public override IAuthHeaderGenerator GetAuthHeaderGenerator(UsernameAndPasswordLoginViewModel loginViewModel)
        {
            return new TDMSecureAuthHeaderGenerator(loginViewModel.Username, loginViewModel.Password, TDMApp.RunningApp.LocalStorageEncryptionKey);
        }
        #endregion
    }
}
