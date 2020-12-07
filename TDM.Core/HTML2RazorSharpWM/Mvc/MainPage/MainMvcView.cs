#nullable enable

using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace HTML2RazorSharpWM.Mvc.MainPage
{
    public class MainMvcView : MvcView
    {
        #region Methods
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
                    new Textarea(new { id="input-text-area" })
                },
                new Div
                {
                    new H2
                    {
                        new Txt("Output (RazorSharp)")
                    },
                    new Textarea( new { id="output-text-area", @readonly="" })
                }
            };

            return ApplyToDefaultLayout(html);
        }
        #endregion
    }
}