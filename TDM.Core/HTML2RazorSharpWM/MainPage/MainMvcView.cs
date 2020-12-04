#nullable enable

using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace HTML2RazorSharpWM.MainPage
{
    public class MainMvcView : MvcView
    {
        public IGenerateHtml RenderIndex()
        {
            var html = new Tags
            {
                new Div
                {
                    new H2
                    {
                        new Txt("Input (HTML)")
                    },
                    new Textarea()
                },
                new Div
                {
                    new H2
                    {
                        new Txt("Output (RazorSharp)")
                    },
                    new Textarea()
                }
            };

            return ApplyToDefaultLayout(html);
        }
    }
}