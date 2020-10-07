﻿#nullable enable

using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace WebMonkTester.Layouts
{
    public class MasterLayoutMvcView : IMvcLayout
    {
        public virtual IGenerateHtml RenderDefaultLayout()
        {
            var html = new Html
            {
                new Head
                {
                    new Title { new Txt("Web Monk Test Project") },
                    new Link(new { rel="stylesheet", href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css", integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh", crossorigin="anonymous" }),
                    new Link(new { rel="stylesheet", href="/css/site.css" }),
                    new Link(new { type="image/x-icon", rel="shortcut icon", href="/images/favicon.ico" }),
                },
                new Body
                {
                    new Script(new { src="https://code.jquery.com/jquery-3.4.1.slim.min.js", integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n", crossorigin="anonymous" }),
                    new Script(new { src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js", integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo", crossorigin="anonymous" }),
                    new Script(new { src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js", integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6", crossorigin="anonymous" }),

                    new BodySectionPlaceholder()
                },
            };

            return html;
        }
    }
}
