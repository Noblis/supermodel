#nullable enable

using WebMonk.HttpRequestHandlers;

namespace WMWeb.Mvc
{
    public class DefaultPathRedirectorHttpRequestHandler : DefaultPathRedirectorHttpRequestHandlerBase
    {
        public DefaultPathRedirectorHttpRequestHandler() : base("/Home/Index") { }
    }
}
