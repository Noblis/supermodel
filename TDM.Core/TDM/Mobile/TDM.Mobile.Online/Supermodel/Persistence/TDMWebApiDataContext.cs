#nullable enable

using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;

namespace TDM.Mobile.Supermodel.Persistence
{
    public class TDMWebApiDataContext : WebApiDataContext
    {
        #region Overrides
        public override string BaseUrl => "http://10.211.55.9:54326/api/"; //this one is for WM
        //public override string BaseUrl => "http://10.211.55.9:50794/";
        #endregion
    }
}
