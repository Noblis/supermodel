#nullable enable

using System;
using System.Net.Http;
using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;

namespace ApiClientCmd.Supermodel.Persistence
{
    public class TDMWebApiDataContext : WebApiDataContext
    {
        #region Overrides
        public override string BaseUrl => "http://10.211.55.9:54326/api/"; //this one is for WM
        //public override string BaseUrl => "http://10.211.55.9:50794/";

        protected override HttpClient CreateHttpClient()
        {
            var httpClient = base.CreateHttpClient();
            httpClient.Timeout = new TimeSpan(0, 10, 0); // set timeout to 10 min, so we can debug
            return httpClient;
        }
        #endregion
    }
}
