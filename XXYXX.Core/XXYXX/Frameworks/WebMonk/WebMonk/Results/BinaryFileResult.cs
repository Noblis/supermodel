#nullable enable

using System.Net;
using System.Threading.Tasks;
using WebMonk.Context;

namespace WebMonk.Results
{
    public class BinaryFileResult : ActionResult
    {
        #region Constructors
        public BinaryFileResult(byte[] body, string fileName, string contentType = "application/octet-stream")
        {
            Body = body;
            FileName = fileName;
            ContentType = contentType;
        }
        #endregion
        
        #region Overrides
        public override async Task ExecuteResultAsync()
        {
            var response = HttpContext.Current.HttpListenerContext.Response;

            response.ContentType = ContentType;
            response.StatusCode = (int)StatusCode;
            response.AddHeader("Content-Disposition", $"Attachment; filename=\"{FileName}\"");

            await response.OutputStream.WriteAsync(Body, 0, Body.Length).ConfigureAwait(false);
        }
        #endregion

        #region Properties
        public byte[] Body { get; }
        public string FileName { get; }
        public string ContentType { get; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        #endregion
    }
}
