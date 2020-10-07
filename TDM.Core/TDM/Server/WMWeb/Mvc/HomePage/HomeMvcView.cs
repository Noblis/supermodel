#nullable enable

using WebMonk.Filters;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace WMWeb.Mvc.HomePage
{
    public class HomeMvcView : MvcView
    {
        [Authorize]
        public IGenerateHtml RenderIndex()
        {
            return ApplyToDefaultLayout(new H2 { new Txt("Welcome to Home Page") });
        }
    }
}
