using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.UnitOfWork;
using WebMonk;
using WebWM.Mvc.Layouts;
using WebWM.Supermodel.Auth;

namespace WebWM
{
    class Program
    {
        static async Task Main()
        {
            //Comment this out if you don't want to recreate and re-seed db every time you start the app in debug mode
            await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
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

            const int port = 54208;
            var webServer = new WebServer(port, $"http://localhost:{port}")
            {
                LoginUrl = "/Auth/Login",
                DefaultLayout = new MasterMvcLayout()
            };
            //webServer.GlobalFilters.Add(new ApiSecureAuthenticateAttribute());
            webServer.GlobalFilters.Add(new ApiBasicAuthenticateAttribute());

            if (Debugger.IsAttached)
            {
                await webServer.RunAsync("/");
                //await webServer.RunAsync();
            }
            else
            {
                await webServer.RunAsync();
            }
        }
    }
}
