using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MvcCoreTester
{
    public class Program
    {
        public static void Main(string[] args)
        //public static async Task Main(string[] args)
        {
            //await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
            //{
            //    await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
            //    await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
            //    await UnitOfWorkContext.SeedDataAsync();
            //}

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .Build()
                .Run();
        }
    }
}
