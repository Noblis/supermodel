#nullable enable

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.UnitOfWork;
using WebMonk;
using WMDomain.Supermodel.Persistence;
using WMWeb.Mvc.Layouts;
using WMWeb.Supermodel.Auth;

namespace WMWeb
{
    class Program
    {
        static async Task Main()
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

            var webServer = new WebServer(54326, "http://localhost:54326") { LoginUrl = "/Auth/Login" };
            
            webServer.GlobalFilters.Add(new WMApiSecureAuthenticateAttribute());
            //webServer.GlobalFilters.Add(new WMApiBasicAuthenticateAttribute());
            
            #if DEBUG
            webServer.ShowErrorDetails = true;
            #endif
            webServer.DefaultLayout = new MasterMvcLayout();

            //await webServer.RunAsync("/Auth/Login");
            await webServer.RunAsync("/");
            //await webServer.RunAsync();
        }
    }
}
