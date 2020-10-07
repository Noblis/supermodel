#nullable enable

using WebMonk.Extensions;
using WebMonk.Filters;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace WebWM.Mvc.HomePage
{
    [Authorize]
    public class HomeMvcController: MvcController
    {
        public ActionResult GetIndex()
        {
            return new HomeMvcView().RenderIndex().ToHtmlResult();
        }
    }
}
