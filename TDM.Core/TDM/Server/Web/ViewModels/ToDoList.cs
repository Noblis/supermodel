#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Models;
using Supermodel.Presentation.Mvc.Models.Api;
using Supermodel.ReflectionMapper;
using Web.ApiControllers;

namespace Web.ViewModels
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
        public Bs4.AutocompleteTextBoxMvcModel<ToDoList, ToDoListAutocompleteApiController, DataContext> SearchTerm { get; set; } = new ();
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
}

