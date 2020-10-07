#nullable enable

using System.ComponentModel.DataAnnotations;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Models;
using Supermodel.ReflectionMapper;
using WMDomain.Entities;
using WMWeb.Mvc.ToDoItemPage;

namespace WMWeb.Mvc.ToDoListPage
{
    public class ToDoListMvcModel : Bs4.MvcModelForEntity<ToDoList>
    {
        #region Overrides
        public override string Label => Name.Value;
        #endregion

        #region Properties
        [Required] public Bs4.TextBoxMvcModel Name { get; set; } = new Bs4.TextBoxMvcModel();
        [ScaffoldColumn(false), NotRMappedTo] public ListViewModel<ToDoItemMvcModel, ToDoItem> ToDoItems { get; set; } = new ListViewModel<ToDoItemMvcModel, ToDoItem>();
        #endregion
    }
}
