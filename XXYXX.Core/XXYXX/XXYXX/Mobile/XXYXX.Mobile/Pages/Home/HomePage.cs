#nullable enable

using System;
using System.Collections.ObjectModel;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using Supermodel.Mobile.Runtime.Common.XForms.Views;
using Xamarin.Forms;
using XXYXX.Mobile.AppCore;
using XXYXX.Mobile.Pages.ChangePassword;
//using XXYXXUserUpdatePassword = XXYXX.Mobile.Models.XXYXXUserUpdatePassword;

namespace XXYXX.Mobile.Pages.Home
{
    public class HomePage : ContentPage
    {
        #region Constructors
        public HomePage()
        {
            var section = new TableSection();
            var cell = new TextCell { Text = "Change Password >" };
            
            #pragma warning disable CS0618 // Type or member is obsolete
            #pragma warning disable 612
            if (Device.OS == TargetPlatform.iOS) cell.TextColor = Color.FromHex("#007AFF");
            #pragma warning restore 612
            #pragma warning restore CS0618 // Type or member is obsolete

            cell.Tapped += async (sender, args) =>
            {
                if (!FormsApplication<XXYXXApp>.RunningApp.AuthHeaderGenerator.UserId.HasValue) throw new Exception("AuthHeaderGenerator.UserId must have value");
                var model = new XXYXXUserUpdatePassword { Id = FormsApplication<XXYXXApp>.RunningApp.AuthHeaderGenerator.UserId.Value };
                await FormsApplication<XXYXXApp>.RunningApp.LoginPage.Navigation.PushAsync(await new ChangePasswordPage().InitAsync(new ObservableCollection<XXYXXUserUpdatePassword> { model }, "Change Password", model));
            };
            section.Add(cell);

            Content = new ViewWithActivityIndicator<TableView>(new TableView { Intent = TableIntent.Form, HasUnevenRows = true, Root = new TableRoot{ section } });
        }
        #endregion
    }
}
