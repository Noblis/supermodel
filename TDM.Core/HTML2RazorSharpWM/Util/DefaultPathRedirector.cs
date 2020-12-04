#nullable enable

using WebMonk.HttpRequestHandlers;

namespace HTML2RazorSharpWM.Util
{
    public class DefaultPathRedirector : DefaultPathRedirectorHttpRequestHandlerBase
    {
        public DefaultPathRedirector() : base("/main/index") { }
    }
}