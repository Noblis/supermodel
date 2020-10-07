#nullable enable

using System.Threading.Tasks;
using Supermodel.Persistence.EFCore.SQLite;

namespace MvcCoreTester.DataContexts
{
    //public class SqlServerDataContext : EFCoreSQLServerDataContext
    //{
    //    public SqlServerDataContext() : base(@"Data Source=.\SQL_DEVELOPER; Initial Catalog=EFCoreTesterDb; Trusted_Connection=True"){}

    //    protected override void SeedData(ModelBuilder modelBuilder)
    //    {
    //         return DataSeeder.SeedDataAsync();
    //    }
    //}

    public class DataContext : EFCoreSQLiteDataContext
    {
        public DataContext() : base($"$Filename={DbFilePath}EFCoreTester.db"){}
        //public SqliteDbContext() : base("Filename=EFCoreTester.db", new CustomRepoFactory()){}
        //public SqliteDbContext() : base("DataSource=:memory:", new CustomRepoFactory()){}

        public override Task SeedDataAsync()
        {
            return DataSeeder.SeedDataAsync();
        }
    }

}