using System.Threading.Tasks;
using EFCoreTester.Repos;
using Supermodel.Persistence.EFCore.InMemory;

namespace EFCoreTester.DataContexts
{
    public class InMemoryDbContext : EFCoreInMemoryDataContext
    {
        public InMemoryDbContext() : base("EFCoreTesterDb", new CustomRepoFactory()){}

        public override Task SeedDataAsync()
        {
            return DataSeeder.SeedDataAsync();
        }
    }
}
