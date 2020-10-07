#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.D3.Models;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace WebWM.Mvc.Layouts
{
    public class EmptyMvcLayout : IMvcLayout
    {
        public virtual IGenerateHtml RenderDefaultLayout()
        {
            return new Html(new { lang="en" })
            {
                new D3.HeadContainer
                {
                    new Title { new Txt("XXYXX") },
                    new Link(new { type="image/x-icon", rel="shortcut icon", href="/images/favicon.ico" }),
                    new Link(new { rel="stylesheet", href="/css/site.css" })
                },
                new D3.BodyContainer()
                {
                    new BodySectionPlaceholder()
                }
            };
        }
    }
}
