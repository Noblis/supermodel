#nullable enable

using System;
using Supermodel.DataAnnotations;
using WebMonk.RazorSharp.HtmlTags.BaseTags;

namespace WebMonk.RazorSharp.HtmlTags
{
    public class Script : Tag
    {
        #region Constructors
        public Script(object? attributes = null) : base("script", attributes) { }
        #endregion

        #region Overrides
        public override StringBuilderWithIndents ToHtml(StringBuilderWithIndents? sb = null)
        {
            sb ??= new StringBuilderWithIndents();

            if (ContainsInnerHtml())
            {
                sb.AppendLineIndentPlus($"<{Name}{GenerateMyAttributesString()}>");
                foreach (var tag in this) 
                {
                    if (tag is Txt txtTag) sb = txtTag.ToHtmlNoHtmlEncode(sb);
                    else throw new SystemException("Script tag can only contain Txt elements");
                }
                sb.AppendLineIndentMinus($"</{Name}>");
            }
            else
            {
                sb.AppendLine($"<{Name}{GenerateMyAttributesString()}></{Name}>");
            }

            return sb;
        }
        #endregion

    }
}
