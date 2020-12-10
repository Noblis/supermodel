#nullable enable

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.UnitOfWork;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (Debugger.IsAttached || !await EFCoreUnitOfWorkContext.Database.CanConnectAsync())
            {
                Console.Write("Recreating the database... ");
                await using (new UnitOfWork<DataContext>())
                {
                    await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
                    await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
                    await UnitOfWorkContext.SeedDataAsync();
                }
                Console.WriteLine("Done!");
            }

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
