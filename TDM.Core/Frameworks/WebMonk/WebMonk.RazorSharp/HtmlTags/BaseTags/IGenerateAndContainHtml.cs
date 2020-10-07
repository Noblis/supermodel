#nullable enable

using System.Collections.Generic;

namespace WebMonk.RazorSharp.HtmlTags.BaseTags
{
    public interface  IGenerateAndContainHtml : IGenerateHtml, IList<IGenerateHtml> { }
}

