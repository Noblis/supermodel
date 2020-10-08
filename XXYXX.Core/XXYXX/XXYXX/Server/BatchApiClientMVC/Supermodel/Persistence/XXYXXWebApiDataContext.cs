using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;

#nullable enable

namespace BatchApiClientMVC.Supermodel.Persistence
{
    public class XXYXXWebApiDataContext: WebApiDataContext
    {
        #region Overrides
        public override string BaseUrl => "http://10.211.55.9:54208/"; //this one is for MVC

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
