#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace WMWeb.Mvc.Layouts
{
    public class EmptyMvcLayout : IMvcLayout
    {
        public virtual IGenerateHtml RenderDefaultLayout()
        {
            return new Html(new { lang="en" })
            {
                new Bs4.HeadContainer
                {
                    new Title { new Txt("WM.TDM.Core") },
                    new Link(new { rel="stylesheet", href="/css/site.css" })
                },
                new Bs4.BodyContainer(new { style="margin: 10px" })
                {
                    new BodySectionPlaceholder()
                }
            };
        }
    }
}
