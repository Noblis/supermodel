#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using Supermodel.Presentation.WebMonk.Models;
using Supermodel.Presentation.WebMonk.Models.Api;
using WMDomain.Entities;

namespace WMWeb.Api.ToDoListApi
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
}
