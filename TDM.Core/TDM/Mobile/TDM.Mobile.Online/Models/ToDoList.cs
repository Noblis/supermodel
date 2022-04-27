using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using Supermodel.Mobile.Runtime.Common.DataContext.Core;
using Supermodel.Mobile.Runtime.Common.UnitOfWork;
using Supermodel.Mobile.Runtime.Common.XForms.UIComponents;
using Supermodel.Mobile.Runtime.Common.XForms.ViewModels;
using TDM.Mobile.AppCore;
using TDM.Mobile.Pages.MyToDoListItemDetail;
using TDM.Mobile.Supermodel.Persistence;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace Supermodel.ApiClient.Models
{
    public class ToDoListXFModel : XFModelForModel<ToDoList>
    {
        #region Overrdies
        public override List<Cell> RenderDetail(Page parentPage, int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            var cells = new List<Cell>();

            if (screenOrderFrom <= 100 && screenOrderTo >= 100)
            {
                cells.AddRange(base.RenderDetail(parentPage, screenOrderFrom, screenOrderTo));
            }

            if (screenOrderFrom <= 200 && screenOrderTo >= 200)
            {
                //Prepare params
                var toDoLists = TDMApp.RunningApp.MyToDoListListPage.Models;
                var sortedItems = ToDoItems.OrderBy(x => x.Completed).ThenBy(x => x.DueOn).ThenBy(x => x.Priority).ToList();

                var childCells = RenderDeletableChildCells<ToDoItem, TDMWebApiDataContext>(parentPage, sortedItems, toDoLists, async (pg, item) =>
                {
                    var itemDetailPage = TDMApp.RunningApp.MyToDoListItemDetailPage = new MyToDoListItemDetailPage();
                    await itemDetailPage.InitAsync(toDoLists, "To Do Item", Model, item.ChildGuidIdentity, item.ParentGuidIdentities);
                    await pg.Navigation.PushAsync(itemDetailPage);
                });
                cells.AddRange(childCells);
            }
            return cells;
        }
        #endregion

        #region Properties
        [Required] public TextBoxXFModel Name { get; set; } = new TextBoxXFModel();
        //[ScaffoldColumn(false)/*, RMCopyShallow*/] public ListViewModel<ToDoItem> ToDoItems { get; set; } = new ListViewModel<ToDoItem>();
        //[ScaffoldColumn(false), RMCopyShallow] public List<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
        [ScaffoldColumn(false)] public List<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
        #endregion
    }

    public partial class ToDoList
    {
        #region Constructors
        public ToDoList()
        {
            CreatedOnUtc = ModifiedOnUtc = DateTime.UtcNow;
        }
        #endregion

        #region Overrdies
        public override List<TChildModel> GetChildList<TChildModel>(params Guid[] parentIdentities)
        {
            if (parentIdentities == null) throw new ArgumentNullException(nameof(parentIdentities));
            if (parentIdentities.Length != 0) throw new Exception("ToDoList.GetChildList(): parentIdentities.Length != 0");
            return (List<TChildModel>)(IEnumerable<TChildModel>)ToDoItems;
        }
        //public override void AfterLoad()
        //{
        //    foreach(var toDoItem in ToDoItems) toDoItem.ParentIdentities = new Guid[0];
        //    base.AfterLoad();
        //}
        public override void BeforeSave(PendingAction.OperationEnum operation)
        {
            if (IsNew)
            {
                var userId = TDMApp.RunningApp.AuthHeaderGenerator.UserId;
                ListOwnerId = userId ?? throw new Exception("TDMApp.RunningApp.AuthHeaderGenerator.UserId == null");
            }
            if (!UnitOfWorkContext.CustomValues.ContainsKey("InSynchronizer")) ModifiedOnUtc = DateTime.UtcNow;
        }

        public override Cell ReturnACell()
        {
            return new ImageCell();
        }
        public override void SetUpBindings(DataTemplate dataTemplate)
        {
            // ReSharper disable AccessToStaticMemberViaDerivedType
            dataTemplate.SetBinding(ImageCell.TextProperty, nameof(Name));
            dataTemplate.SetBinding(ImageCell.DetailProperty, nameof(ItemsCount));
            // ReSharper restore AccessToStaticMemberViaDerivedType

            dataTemplate.SetBinding(ImageCell.ImageSourceProperty, nameof(IconImageSource));
        }
        #endregion

        #region Properties
        [JsonIgnore] public ImageSource IconImageSource => ImageSource.FromResource("TDM.Mobile.EmbeddedResources.List.png");
        [JsonIgnore] public string ItemsCount => $"Completed: {ToDoItems.Count(x => x.Completed)} Pending:{ToDoItems.Count(x => !x.Completed)}";
        #endregion
    }
}
