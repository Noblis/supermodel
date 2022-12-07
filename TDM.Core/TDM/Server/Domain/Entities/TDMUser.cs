#nullable enable

using System.Collections.Generic;
using System.Linq;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence;
using Supermodel.Persistence.Entities;

namespace Domain.Entities
{
    public class TDMUser : UserEntity<TDMUser, DataContext>
    {
        #region Overrides
        protected override void DeleteInternal()
        {
            if (ToDoLists.Any()) throw new UnableToDeleteException("User contains To Do Lists"); 
            base.DeleteInternal();
        }
        #endregion

        #region Properties
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public virtual List<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
        #endregion
    }
}
