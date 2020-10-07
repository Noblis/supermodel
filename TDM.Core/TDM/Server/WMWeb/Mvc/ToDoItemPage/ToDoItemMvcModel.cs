#nullable enable

using System.ComponentModel.DataAnnotations;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using WMDomain.Entities;

namespace WMWeb.Mvc.ToDoItemPage
{
    public class ToDoItemMvcModel : Bs4.ChildMvcModelForEntity<ToDoItem, ToDoList>
    {
        #region Overrides
        public override string Label => Name.Value + (Completed.ValueBool ? " (Completed)" : "");
        public override ToDoList? GetParentEntity(ToDoItem entity)
        {
            return entity.ParentToDoList;
        }
        public override void SetParentEntity(ToDoItem entity, ToDoList? parent)
        {
            entity.ParentToDoList = parent;
        }
        #endregion

        #region Properties
        [Required] public Bs4.TextBoxMvcModel Name { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.DropdownMvcModelUsingEnum<PriorityEnum> Priority { get; set; } = new Bs4.DropdownMvcModelUsingEnum<PriorityEnum>();
        public Bs4.DateMvcModel DueOn { get; set; } = new Bs4.DateMvcModel();
        public Bs4.CheckboxMvcModel Completed { get; set; } = new Bs4.CheckboxMvcModel();
        #endregion
    }
}
