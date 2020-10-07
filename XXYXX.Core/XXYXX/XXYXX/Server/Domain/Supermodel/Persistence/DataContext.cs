#nullable enable

using System.Threading.Tasks;
using Supermodel.Persistence.EFCore.SQLite;

namespace Domain.Supermodel.Persistence
{
    //Pick one of the below data contexts for your application
    
    //Sqlite db
    public class DataContext : EFCoreSQLiteDataContext
    {
        #region Constructors
        public DataContext() : base($"Filename={DbFilePath}XXYXX.SQLite.db", new CustomRepoFactory()) { }
        #endregion

        #region Overrides
        public override Task SeedDataAsync()
        {
            return DataSeeder.SeedDataAsync();
        }
        #endregion
    }

    //Sqlite in memory db
    //public class DataContext : EFCoreSQLiteDataContext
    //{
    //    #region Constructors
    //    public DataContext() : base("DataSource=:memory:", new CustomRepoFactory()) { }
    //    #endregion

    //    #region Overrides
    //    public override Task SeedDataAsync()
    //    {
    //        return DataSeeder.SeedDataAsync();
    //    }
    //    #endregion
    //}

    //Sql Server db
    //public class DataContext : EFCoreSQLServerDataContext
    //{
    //    #region Constructors
    //    public DataContext() : base(@"Data Source=.\SQL_DEVELOPER; Initial Catalog=XXYXXDb; Trusted_Connection=True", new CustomRepoFactory()) { }
    //    #endregion

    //    #region Overrides
    //    public override Task SeedDataAsync()
    //    {
    //        return DataSeeder.SeedDataAsync();
    //    }
    //    #endregion
    //}
}
