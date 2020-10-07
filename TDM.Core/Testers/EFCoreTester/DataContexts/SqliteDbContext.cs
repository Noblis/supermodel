using System.Threading.Tasks;
using EFCoreTester.Repos;
using Supermodel.Persistence.EFCore.SQLite;

namespace EFCoreTester.DataContexts
{
    public class SqliteDbContext : EFCoreSQLiteDataContext
    {
        public SqliteDbContext() : base($"Filename={DbFilePath}EFCoreTester.db", new CustomRepoFactory()){}
        //public SqliteDbContext() : base("DataSource=:memory:", new CustomRepoFactory()){}

        public override Task SeedDataAsync()
        {
            return DataSeeder.SeedDataAsync();
        }
    }
}