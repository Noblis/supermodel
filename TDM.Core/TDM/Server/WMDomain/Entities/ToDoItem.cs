#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using Supermodel.Persistence.Entities;

namespace WMDomain.Entities
{   
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
