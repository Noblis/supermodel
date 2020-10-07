#nullable enable

using WebMonk.Extensions;
using WebMonk.Filters;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace WMWeb.Mvc.HomePage
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
