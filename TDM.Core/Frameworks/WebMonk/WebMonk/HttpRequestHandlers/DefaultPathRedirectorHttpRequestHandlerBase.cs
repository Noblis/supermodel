#nullable enable

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebMonk.Context;

namespace WebMonk.HttpRequestHandlers
{
    public abstract class DefaultPathRedirectorHttpRequestHandlerBase : IHttpRequestHandler
    {
        #region Constructors
        protected DefaultPathRedirectorHttpRequestHandlerBase(string redirectTo)
        {
            RedirectTo = redirectTo;
        }
        #endregion
        
        #region Overrides
        public virtual Task<IHttpRequestHandler.HttpRequestHandlerResult> TryExecuteHttpRequestAsync(CancellationToken cancellationToken)
        {
            IHttpRequestHandler.HttpRequestHandlerResult result;
            if (HttpContext.Current.RouteManager.LocalPath != "/") 
            {
                 result = IHttpRequestHandler.HttpRequestHandlerResult.False;
            }
            else
            {
                result = new IHttpRequestHandler.HttpRequestHandlerResult(true, async () => 
                { 
                    //We do this because we want to preserve TempData through multiple redirects
                    HttpContext.Current.TempData.MergeCurrentIntoFuture(HttpContext.Current.SessionId);

                    var response = HttpContext.Current.HttpListenerContext.Response;
                    response.StatusCode = (int)HttpStatusCode.Redirect;
                    response.RedirectLocation = RedirectTo;
                    await response.OutputStream.WriteAsync(new byte[0], 0, 0, cancellationToken).ConfigureAwait(false);
                });
            }
            return Task.FromResult(result);
        }
        public virtual int Priority => 100;
        public virtual bool SaveSessionState => false;
        #endregion

        #region Properties
        public string RedirectTo { get; }
        #endregion
    }
}
