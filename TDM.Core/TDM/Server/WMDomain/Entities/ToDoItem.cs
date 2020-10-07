#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using Supermodel.Persistence.Entities;

namespace WMDomain.Entities
{   
    public class ToDoItem : Entity
    {
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
