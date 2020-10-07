#nullable enable

using Supermodel.Mobile.Runtime.Common.XForms;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using Xamarin.Forms;
using XXYXX.Mobile.Pages.ChangePassword;
using XXYXX.Mobile.Pages.Home;
using XXYXX.Mobile.Pages.Login;

namespace XXYXX.Mobile.AppCore
{
    public class XXYXXApp : SupermodelXamarinFormsApp
    {
        #region Constructors
        public XXYXXApp()
        {
            XFormsSettings.LabelFontSize = XFormsSettings.ValueFontSize = 18;

            LoginPage = new LoginPage();
            MainPage = new NavigationPage(LoginPage);

            #pragma warning disable 4014
            LoginPage.AutoLoginIfPossibleAsync();
            #pragma warning restore 4014

            //Init these properties here because we are in #nullable enable context
            HomePage = new HomePage();
            ChangePasswordPage = new ChangePasswordPage();
        }
        #endregion

        #region Overrides
        public override void HandleUnauthorized()
        {
            LoginPage = new LoginPage();
            MainPage = new NavigationPage(FormsApplication<XXYXXApp>.RunningApp.LoginPage);
        }

        public override byte[] LocalStorageEncryptionKey { get; } = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; //TODO: This should have been replaced by the VS plugin
        #endregion

        #region Methods
        public static XXYXXApp RunningApp => FormsApplication<XXYXXApp>.RunningApp;
        #endregion

        #region Properties
        public LoginPage LoginPage { get; set; }
        public HomePage HomePage { get; set; }
        public ChangePasswordPage ChangePasswordPage { get; set; }
        #endregion
    }
}
