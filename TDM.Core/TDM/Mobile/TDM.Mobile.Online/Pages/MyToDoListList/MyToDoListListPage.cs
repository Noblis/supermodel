using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.App;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.CRUDList;
using TDM.Mobile.AppCore;
using TDM.Mobile.Pages.MyToDoListDetail;
using TDM.Mobile.Pages.Settings;
using TDM.Mobile.Supermodel.Persistence;
using Xamarin.Forms;

namespace TDM.Mobile.Pages.MyToDoListList
{
    public class MyToDoListListPage : EnhancedCRUDListPage<ToDoList, TDMWebApiDataContext>
    {
        #region Constrcuctors
        public MyToDoListListPage()
        {
            TDMApp.RunningApp.MyToDoListListPage = this;

            var settingsToolbarItem = new ToolbarItem("Settings", "settings.png", async() => 
            {
                var settingsPage = FormsApplication<TDMApp>.RunningApp.SettingsPage = new SettingsPage();
                await Navigation.PushAsync(settingsPage);
            });
            ToolbarItems.Add(settingsToolbarItem);
        }
        #endregion

        #region Overrdies
        protected override async Task OpenDetailInternalAsync(ToDoList model)
        {
            var detailPage = TDMApp.RunningApp.MyToDoListDetailPage = (MyToDoListDetailPage)await new MyToDoListDetailPage().InitAsync(Models, model.IsNew ? "New List" : "Edit List", model);
            await Navigation.PushAsync(detailPage);
        }
        protected override string NewBtnIconFilename => "plus.png";
        #endregion
    }
}
