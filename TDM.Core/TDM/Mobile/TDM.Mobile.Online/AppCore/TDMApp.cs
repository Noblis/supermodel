using Supermodel.Mobile.Runtime.Common.XForms;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using TDM.Mobile.Pages.ChangePassword;
using TDM.Mobile.Pages.Login;
using TDM.Mobile.Pages.MyToDoListDetail;
using TDM.Mobile.Pages.MyToDoListItemDetail;
using TDM.Mobile.Pages.MyToDoListList;
using TDM.Mobile.Pages.Settings;
using Xamarin.Forms;

namespace TDM.Mobile.AppCore
{
    public class TDMApp : SupermodelXamarinFormsApp
    {
        #region Constructors
        public TDMApp()
        {
            XFormsSettings.LabelFontSize = XFormsSettings.ValueFontSize = 18;

            LoginPage = new LoginPage();
            MainPage = new NavigationPage(LoginPage);

            #pragma warning disable 4014
            LoginPage.AutoLoginIfPossibleAsync();
            #pragma warning restore 4014
        }
        #endregion

        #region Overrides
        public override void HandleUnauthorized()
        {
            LoginPage = new LoginPage();
            MainPage = new NavigationPage(FormsApplication<TDMApp>.RunningApp.LoginPage);
        }

        public override byte[] LocalStorageEncryptionKey { get; } = { 0x5A, 0x56, 0x8D, 0x33, 0x9C, 0xF6, 0x76, 0x84, 0xC7, 0x00, 0xAA, 0x9D, 0x71, 0x68, 0xE0, 0xCB }; //Randomly genrated by Supermodel VS plugin
        #endregion

        #region Methods
        public static TDMApp RunningApp => FormsApplication<TDMApp>.RunningApp;
        #endregion

        #region Properties
        public LoginPage LoginPage { get; set; }
        public MyToDoListListPage MyToDoListListPage { get; set; }
        public MyToDoListDetailPage MyToDoListDetailPage { get; set; }
        public MyToDoListItemDetailPage MyToDoListItemDetailPage { get; set; }
        public SettingsPage SettingsPage { get; set; }
        public ChangePasswordPage ChangePasswordPage { get; set; }
        #endregion
    }
}
