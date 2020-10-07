using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Supermodel.Mobile.Runtime.Common.Models;
using Supermodel.Mobile.Runtime.Common.XForms.UIComponents;
using Supermodel.Mobile.Runtime.Common.XForms.ViewModels;
using Supermodel.ReflectionMapper;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace Supermodel.ApiClient.Models
{
    //Note that this is XFModel for ToDoList Model
    public class ToDoListItemXFModel : XFModelForChildModel<ToDoList, ToDoItem>
    {
        #region Methods
        public void UpdateActive()
        {
            Name.Active = Priority.Active = DueOn.Active = !Completed.IsToggled;
        }
        #endregion

        #region Properties
        [Required] public TextBoxXFModel Name { get; set; } = new TextBoxXFModel();
        public DateXFModel DueOn { get; set; } = new DateXFModel();
        public DropdownXFModelUsingEnum<PriorityEnum> Priority { get; set; } = new DropdownXFModelUsingEnum<PriorityEnum>();
        public ToggleSwitchXFModel Completed { get; set; } = new ToggleSwitchXFModel
        {
            OnChanged = page =>
            {
                page.GetXFModel<ToDoListItemXFModel>().UpdateActive();
            }
        };
        #endregion
    }

    public partial class ToDoItem : DirectChildModel
    {
        #region Overrides
        public override Cell ReturnACell()
        {
            return new ImageCell();
        }
        public override void SetUpBindings(DataTemplate dataTemplate)
        {
            // ReSharper disable AccessToStaticMemberViaDerivedType
            dataTemplate.SetBinding(ImageCell.TextProperty, nameof(Name));
            dataTemplate.SetBinding(ImageCell.DetailProperty, nameof(ItemsDetail));
            // ReSharper restore AccessToStaticMemberViaDerivedType

            dataTemplate.SetBinding(ImageCell.ImageSourceProperty, nameof(IconImageSource));
        }
        #endregion

        #region Properties
        [JsonIgnore, NotRMapped] public ImageSource IconImageSource => Completed ? ImageSource.FromResource("TDM.Mobile.EmbeddedResources.Checked.png") : ImageSource.FromResource("TDM.Mobile.EmbeddedResources.Unchecked.png");
        [JsonIgnore, NotRMapped] public string ItemsDetail => $"Due: {DueOn?.ToString("d")}  Priority: {Priority}";
        #endregion
    }
}