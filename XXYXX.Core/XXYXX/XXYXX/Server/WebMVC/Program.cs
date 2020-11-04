#nullable enable

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.UnitOfWork;

namespace WebMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Comment this out if you don't want to recreate and re-seed db every time you start the app in debug mode
            if (Debugger.IsAttached)
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

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    }
}
