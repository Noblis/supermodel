#nullable enable

using System.Collections.Generic;
using Supermodel.Persistence.Entities;
using WMDomain.Supermodel.Persistence;

namespace WMDomain.Entities
{
    public class TDMUser : UserEntity<TDMUser, DataContext>
    {
        #region Properties
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public virtual List<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
        #endregion
    }
}
