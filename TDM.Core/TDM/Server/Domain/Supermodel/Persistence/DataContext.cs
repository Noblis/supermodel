#nullable enable

using System.Threading.Tasks;
//using Supermodel.Persistence.EFCore.SQLServer;
using Supermodel.Persistence.EFCore.SQLite;

namespace Domain.Supermodel.Persistence
{
    public class DataContext : EFCoreSQLiteDataContext
    {
        #region Constructors
        public DataContext() : base($"Filename={DbFilePath}TDMCore.SQLite.db") { }
        #endregion

        #region Overrides
        public override Task SeedDataAsync()
        {
            return DataSeeder.SeedDataAsync();
        }
        #endregion
    }

    //public class DataContext : EFCoreSQLServerDataContext
    //{
    //    #region Constructors
    //    public DataContext() : base(@"Data Source=.\SQL_DEVELOPER; Initial Catalog=TDMCoreDb; Trusted_Connection=True"){}
    //    #endregion

    //    #region Overrides
    //    protected override void SeedData(ModelBuilder modelBuilder)
    //    {
    //        DataSeeder.SeedData(modelBuilder);
    //    }
    //    #endregion
    //}
}
