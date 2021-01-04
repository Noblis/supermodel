#nullable enable

using WebMonk.HttpRequestHandlers;

namespace HTML2RazorSharpWM.Mvc.Util
{
    public class DefaultPathRedirector : DefaultPathRedirectorHttpRequestHandlerBase
    {
        #region Constructors
        public DefaultPathRedirector() : base("/main/index") { }
        #endregion
    }
}