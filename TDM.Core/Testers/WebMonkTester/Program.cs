#nullable enable

using System.Threading.Tasks;
using WebMonk;
using WebMonkTester.Supermodel.Auth;

namespace WebMonkTester
{
    class Program
    {
        static async Task Main()
        {
            var webServer = new WebServer(54326, "http://localhost:54326") { LoginUrl = "/Auth/Login" };
            webServer.GlobalFilters.Add(new WMApiBasicAuthenticateAttribute());
            webServer.ShowErrorDetails = true;

            await webServer.RunAsync("/");
            //await webServer.RunAsync("Api/Student/List");
            //await webServer.RunAsync("Student/Detail");
            //await webServer.RunAsync("Student/RedirectToDetail");
            //await webServer.RunAsync("css/site.css");
        }
    }
}
