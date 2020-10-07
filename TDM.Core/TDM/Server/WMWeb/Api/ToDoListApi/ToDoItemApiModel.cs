#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using Supermodel.Presentation.WebMonk.Models.Api;
using WMDomain.Entities;

namespace WMWeb.Api.ToDoListApi
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
}
