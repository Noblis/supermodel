#nullable enable

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;

namespace Batch
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            //Comment this out if you don't want to recreate and re-seed db every time you start the app in debug mode
            await using (new UnitOfWork<DataContext>())
            {
                if (Debugger.IsAttached || !await EFCoreUnitOfWorkContext.Database.CanConnectAsync())
                {
                    Console.Write("Recreating the database... ");
                    await EFCoreUnitOfWorkContext.Database.EnsureDeletedAsync();
                    await EFCoreUnitOfWorkContext.Database.EnsureCreatedAsync();
                    await UnitOfWorkContext.SeedDataAsync();
                    Console.WriteLine("Done!");
                }
            }

            await using (new UnitOfWork<DataContext>())
            {
                var repo = RepoFactory.Create<XXYXXUser>();
                var user = await repo.GetByIdOrDefaultAsync(1);
                if (user == null)
                {
                    Console.WriteLine("User with id=1 does not exist!");
                }
                else
                {
                    user.Password = "12345";
                }
            }
            Console.WriteLine("User with id=1's password updated to '12345'");
        }
    }
}
