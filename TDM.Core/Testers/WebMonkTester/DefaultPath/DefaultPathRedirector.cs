#nullable enable

using WebMonk.HttpRequestHandlers;

namespace WebMonkTester.DefaultPath
{
    public class DefaultPathRedirector : DefaultPathRedirectorHttpRequestHandlerBase
    {
        public DefaultPathRedirector() : base("/Student/RedirectToDetail") { }
    }
}
