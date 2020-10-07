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
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Models;
using Supermodel.Presentation.Mvc.Models.Api;
using Supermodel.ReflectionMapper;

namespace Domain.Entities
{
    public class ToDoListApiModel : ApiModelForEntity<ToDoList>
    {
        #region Properties
        [Required] public DateTime CreatedOnUtc { get; set; }
        [Required] public DateTime ModifiedOnUtc { get; set; }

        [Required] public long ListOwnerId { get; set; }
        [Required] public string Name { get; set; } = "";

        public ListViewModel<ToDoItemApiModel, ToDoItem> ToDoItems { get; set; } = new ListViewModel<ToDoItemApiModel, ToDoItem>();
        #endregion
    }
    
    public class ToDoListSearchMvcModel : Bs4.MvcModel
    {
        public Bs4.AutocompleteTextBoxMvcModel SearchTerm { get; set; } = new Bs4.AutocompleteTextBoxMvcModel("ToDoListAutocompleteApi");
    }

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

