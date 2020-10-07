using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.CRUDDetail;
using TDM.Mobile.AppCore;
using TDM.Mobile.Pages.MyToDoListItemDetail;
using TDM.Mobile.Supermodel.Persistence;
using Xamarin.Forms;

namespace TDM.Mobile.Pages.MyToDoListDetail
{
    public class MyToDoListDetailPage : CRUDDetailPage<ToDoList, ToDoListXFModel, TDMWebApiDataContext>
    {
        #region Initializers
        public override async Task<CRUDDetailPage<ToDoList, ToDoListXFModel, TDMWebApiDataContext>> InitAsync(ObservableCollection<ToDoList> models, string title, ToDoList model)
        {
            TDMApp.RunningApp.MyToDoListDetailPage = this;

            var addToolbarItem = new ToolbarItem("Add", "plus.png", async () => 
            {
                var toDoLists = TDMApp.RunningApp.MyToDoListListPage.Models;

                var itemDetailPage = TDMApp.RunningApp.MyToDoListItemDetailPage = new MyToDoListItemDetailPage();
                await itemDetailPage.InitAsync(toDoLists, "To Do Item", model, Guid.NewGuid());
                await Navigation.PushAsync(itemDetailPage);
            });
            ToolbarItems.Add(addToolbarItem);
            return await base.InitAsync(models, title, model);
        }
        #endregion

        #region Overrides
        protected override async Task SaveItemInternalAsync(ToDoList model)
        {
            if (model.IsNew)
            {
                var userId = TDMApp.RunningApp.AuthHeaderGenerator.UserId;
                model.ListOwnerId = userId ?? throw new Exception("TDMApp.RunningApp.AuthHeaderGenerator.UserId == null");
            }
            await base.SaveItemInternalAsync(model);
        }
        public override void InitDetailView()
        {
            var section1 = new TableSection();
            DetailView.ContentView.Root.Add(section1);
            var cells1 = XFModel.RenderDetail(this, 100, 100);
            foreach (var cell in cells1) section1.Add(cell);

            var section2 = new TableSection();
            DetailView.ContentView.Root.Add(section2);
            var cells2 = XFModel.RenderDetail(this, 200, 200);
            foreach (var cell in cells2) section2.Add(cell);
        }
        protected override bool CancelButton => true;
        #endregion
    }
}
