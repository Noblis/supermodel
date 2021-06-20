#nullable enable

using WebMonk.Extensions;
using WebMonk.Filters;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;
using WMWeb.Mvc.HomePage;

namespace WMWeb.Mvc.WarningTestPage
{
    [Authorize]
    public class HomeMvcController : MvcController
    {
        #region Action Methods
        public ActionResult GetIndex()
        {
            return new HomeMvcView().RenderIndex().ToHtmlResult();
        }
        #endregion
    }
}
