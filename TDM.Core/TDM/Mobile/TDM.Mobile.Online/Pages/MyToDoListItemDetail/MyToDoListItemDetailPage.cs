using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.CRUDDetail;
using TDM.Mobile.AppCore;
using TDM.Mobile.Supermodel.Persistence;

namespace TDM.Mobile.Pages.MyToDoListItemDetail
{
    public class MyToDoListItemDetailPage : CRUDChildDetailPage<ToDoList, ToDoItem, ToDoListItemXFModel, TDMWebApiDataContext>
    {
        #region Constructors
        public MyToDoListItemDetailPage()
        {
            TDMApp.RunningApp.MyToDoListItemDetailPage = this;
        }
        #endregion

        #region Overrides
        public override void OnLoad()
        {
            base.OnLoad();
            XFModel.UpdateActive();
        }
        #endregion

        #region Properties
        protected override bool CancelButton => true;
        #endregion
    }
}
