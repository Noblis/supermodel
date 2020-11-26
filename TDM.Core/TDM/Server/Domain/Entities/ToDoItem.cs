#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using Supermodel.Persistence.Entities;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Models.Api;

namespace Domain.Entities
{
    public class ToDoItemApiModel : ApiModelForEntity<ToDoItem>
    {
        #region Properties
        [Required] public string Name { get; set; } = "";
        public PriorityEnum? Priority { get; set; }
        public DateTime? DueOn { get; set; }
        public bool Completed { get; set; }
        #endregion
    }
    
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
        public override NumberOfColumnsEnum NumberOfColumns => NumberOfColumnsEnum.Three;
        #endregion

        #region Properties
        [Required] public Bs4.TextBoxMvcModel Name { get; set; } = new Bs4.TextBoxMvcModel();
        public Bs4.DropdownMvcModelUsingEnum<PriorityEnum> Priority { get; set; } = new Bs4.DropdownMvcModelUsingEnum<PriorityEnum>();
        public Bs4.DateMvcModel DueOn { get; set; } = new Bs4.DateMvcModel();
        public Bs4.CheckboxMvcModel Completed { get; set; } = new Bs4.CheckboxMvcModel();
        #endregion
    }
    
    public class ToDoItem : Entity
    {
        //#region Validation
        //public override async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        //{
        //    var vrl = await base.ValidateAsync(validationContext);
        //    vrl.Add(new ValidationResult("Test Problem"));
        //    return vrl;
        //}
        //#endregion

        #region Properties
        public long ParentToDoListId { get; set; }
        public virtual ToDoList? ParentToDoList { get; set; }

        [Required] public string Name { get; set; } = "";
        public PriorityEnum? Priority { get; set; }
        public DateTime? DueOn { get; set; }
        public bool Completed { get; set; }
        #endregion
    }
}
