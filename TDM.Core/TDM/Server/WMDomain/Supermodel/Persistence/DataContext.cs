#nullable enable

using Supermodel.Persistence.EFCore.SQLite;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
//using Supermodel.Persistence.EFCore.SQLServer;

namespace WMDomain.Supermodel.Persistence
{
    public class DataContext : EFCoreSQLiteDataContext
    {
        #region Constructors
        public DataContext() 
            : base(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)?
                @"Filename=/Users/ilyabasin/Documents/Projects/TDM.Core/TDM/Server/TDMCore.SQLite.db" : 
                @"Filename=/Users/m29598/tdm.core/TDM/Server/TDMCore.SQLite.db") { }
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
