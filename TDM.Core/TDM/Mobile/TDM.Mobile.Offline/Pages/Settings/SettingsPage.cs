using System;
using System.Collections.ObjectModel;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using Supermodel.Mobile.Runtime.Common.XForms.Views;
using TDM.Mobile.AppCore;
using TDM.Mobile.Pages.ChangePassword;
using Xamarin.Forms;

namespace TDM.Mobile.Pages.Settings
{
    public class SettingsPage : ContentPage
    {
        #region Constructors
        public SettingsPage()
        {
            TDMApp.RunningApp.SettingsPage = this;

            var section = new TableSection();
            var cell = new TextCell { Text = "Change Password >" };
            
            if (Device.RuntimePlatform == Device.iOS) cell.TextColor = Color.FromHex("#007AFF");

            cell.Tapped += async (sender, args) =>
            {
                if (!FormsApplication<TDMApp>.RunningApp.AuthHeaderGenerator.UserId.HasValue) throw new Exception("AuthHeaderGenerator.UserId must have value");
                var model = new TDMUserUpdatePassword { Id = FormsApplication<TDMApp>.RunningApp.AuthHeaderGenerator.UserId.Value };
                await FormsApplication<TDMApp>.RunningApp.LoginPage.Navigation.PushAsync(await new ChangePasswordPage().InitAsync(new ObservableCollection<TDMUserUpdatePassword> { model }, "Change Password", model));
            };
            section.Add(cell);

            Content = new ViewWithActivityIndicator<TableView>(new TableView { Intent = TableIntent.Form, HasUnevenRows = true, Root = new TableRoot{ section } });
        }
        #endregion
    }
}
