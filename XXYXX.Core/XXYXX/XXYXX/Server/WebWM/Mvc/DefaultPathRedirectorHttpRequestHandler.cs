#nullable enable

using WebMonk.HttpRequestHandlers;

namespace WebWM.Mvc
{
    public class DefaultPathRedirectorHttpRequestHandler : DefaultPathRedirectorHttpRequestHandlerBase
    {
        public DefaultPathRedirectorHttpRequestHandler() : base("/Auth/Login") { }
    }
}