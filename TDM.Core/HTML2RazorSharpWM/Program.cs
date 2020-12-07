#nullable enable

using System.Threading.Tasks;
using HTML2RazorSharpWM.Mvc.Layouts;
using WebMonk;

namespace HTML2RazorSharpWM
{
    public class Program
    {
        public static async Task Main()
        {
            var webServer = new WebServer(51413, "http://localhost:51413");

            webServer.ShowErrorDetails = true;
            webServer.DefaultLayout = new MasterMvcLayout();

            await webServer.RunAsync("/");
        }
    }
}
