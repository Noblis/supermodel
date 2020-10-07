using WebMonk.RazorSharp.HtmlTags.BaseTags;

#nullable enable

namespace WebMonk.Rendering.Templates
{
    public interface IHiddenTemplate
    { 
        IGenerateHtml HiddenTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null);
    }
}