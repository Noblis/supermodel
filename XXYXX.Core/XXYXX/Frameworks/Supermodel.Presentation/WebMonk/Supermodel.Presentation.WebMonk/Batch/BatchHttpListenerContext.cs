#nullable disable

using WebMonk.Context.WMHttpListenerObjects;

namespace Supermodel.Presentation.WebMonk.Batch
{
    public class BatchHttpListenerContext : IHttpListenerContext
    {
        #region Constructors
        public BatchHttpListenerContext(IHttpListenerContext rootContext, string httpRequestRawStr)
        {
            Request = new BatchHttpListenerRequest(httpRequestRawStr, rootContext.Request);
            Response = new BatchHttpListenerResponse(rootContext.Response);
        }
        #endregion
        
        #region Properties
        public IHttpListenerRequest Request { get; set; }
        public IHttpListenerResponse Response { get; set; }
        #endregion
    }
}
