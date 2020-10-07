#nullable enable

using System.Threading.Tasks;
using EFCoreTester.Repos;
using Supermodel.Persistence.EFCore.SQLServer;

namespace EFCoreTester.DataContexts
{
    public class SqlServerDbContext : EFCoreSQLServerDataContext
    {
        public SqlServerDbContext() : base(@"Data Source=.\SQL_DEVELOPER; Initial Catalog=EFCoreTesterDb; Trusted_Connection=True", new CustomRepoFactory()){}

        public override Task SeedDataAsync()
        {
            return DataSeeder.SeedDataAsync();
        }
    }
}