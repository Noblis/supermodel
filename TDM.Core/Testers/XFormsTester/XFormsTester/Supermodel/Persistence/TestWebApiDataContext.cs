using Supermodel.Mobile.Runtime.Common.DataContext.WebApi;

namespace XFormsTester.Supermodel.Persistence
{
    public class TestWebApiDataContext: WebApiDataContext
    {
        #region Overrides
        public override string BaseUrl => "http://localhost:62268/";
        #endregion
    }
}
