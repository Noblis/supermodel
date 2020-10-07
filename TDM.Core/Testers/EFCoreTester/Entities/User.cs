using EFCoreTester.DataContexts;
using Supermodel.Persistence.Entities;

namespace EFCoreTester.Entities
{
    public class User : UserEntity<User, SqlServerDbContext> 
    {
        //public override string Label => Username;
    }
    //public class User : UserEntityBase<User, SqliteDbContext> {}
    //public class User : UserEntityBase<User, InMemoryDbContext> {}
}
