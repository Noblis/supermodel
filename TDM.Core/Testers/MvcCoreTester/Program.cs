using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MvcCoreTester.DataContexts;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.UnitOfWork;

namespace MvcCoreTester
{
    public class Program
    {
        public static void Main(string[] args)
        //public static async Task Main(string[] args)
        {
            //if (Debugger.IsAttached || !await EFCoreUnitOfWorkContext.Database.CanConnectAsync())
            //{
            //    Console.Write("Recreating the database... ");
            //    await using (new UnitOfWork<DataContext>())
            //    {
            //        await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
            //        await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
            //        await UnitOfWorkContext.SeedDataAsync();
            //    }
            //    Console.WriteLine("Done!");
            //}

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .Build()
                .Run();
        }
    }
}
