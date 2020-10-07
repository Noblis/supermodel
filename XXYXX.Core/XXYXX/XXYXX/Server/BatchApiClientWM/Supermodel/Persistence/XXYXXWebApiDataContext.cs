#nullable enable

using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;

namespace BatchApiClientWM.Supermodel.Persistence
{
    public class XXYXXWebApiDataContext: WebApiDataContext
    {
        #region Overrides
        //public override string BaseUrl => "http://10.211.55.9:54207/"; //this one is for MVC
        public override string BaseUrl => "http://10.211.55.9:54208/api/"; //this one is for WM

        // set timeout to 10 min, so we can debug
        //protected override HttpClient CreateHttpClient()
        //{
        //    var httpClient = base.CreateHttpClient();
        //    httpClient.Timeout = new TimeSpan(0, 10, 0);
        //    return httpClient;
        //}
        #endregion
    }
}
