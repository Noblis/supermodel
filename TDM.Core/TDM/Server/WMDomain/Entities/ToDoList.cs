#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Validations;
using Supermodel.Persistence;
using Supermodel.Persistence.DataContext;
using Supermodel.Persistence.Entities;

namespace WMDomain.Entities
{   
    public class ToDoList : Entity
    {
        #region Constrcutors
        public ToDoList()
        {
            CreatedOnUtc = ModifiedOnUtc = DateTime.UtcNow;
        }
        #endregion

        #region Overrides
        public override async Task<ValidationResultList> ValidateAsync(ValidationContext validationContext)
        {
            var vr = await base.ValidateAsync(validationContext);
            if (Name.Length <= 1) vr.AddValidationResult(this, "Name must be longer than one character", x => x.Name);
            return vr;
        }
        protected override void DeleteInternal()
        {
            //foreach (var toDoItem in ToDoItems.ToList())
            //{
            //    toDoItem.Delete();
            //}
            if (ToDoItems.Any()) throw new UnableToDeleteException("Must delete all items in the To Do List first");
            base.DeleteInternal();
        }
        public override async Task BeforeSaveAsync(OperationEnum operation)
        {
            ModifiedOnUtc = DateTime.UtcNow;
            await base.BeforeSaveAsync(operation);
        }
        #endregion
        
        #region Properties
        [Required] public DateTime CreatedOnUtc { get; set; }
        [Required] public DateTime ModifiedOnUtc { get; set; }

        public long ListOwnerId { get; set; }
        public virtual TDMUser? ListOwner { get; set; }

        [Required] public string Name { get; set; } = "";
        public virtual List<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
        #endregion
    }
}

