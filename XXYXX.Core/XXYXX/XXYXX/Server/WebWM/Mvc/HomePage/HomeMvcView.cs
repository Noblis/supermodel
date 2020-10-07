#nullable enable

using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace WebWM.Mvc.HomePage
{
    public class HomeMvcView : MvcView
    {
        #region View Methods
        public IGenerateHtml RenderIndex()
        {
            var html = new Tags
            {
                new H2
                {
                    new Txt("Welcome to Home Page"),
                },
            };

            return ApplyToDefaultLayout(html);
        }
        #endregion
    }
}
