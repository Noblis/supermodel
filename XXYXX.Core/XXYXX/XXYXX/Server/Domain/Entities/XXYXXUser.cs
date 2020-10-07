#nullable enable

using Domain.Supermodel.Persistence;
using Supermodel.Persistence.Entities;

namespace Domain.Entities
{
    public class XXYXXUser : UserEntity<XXYXXUser, DataContext>
    {
        #region Properties
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        #endregion
    }
}
