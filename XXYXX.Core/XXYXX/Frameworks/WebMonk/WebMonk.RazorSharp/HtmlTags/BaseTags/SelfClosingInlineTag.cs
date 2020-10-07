#nullable enable

using Supermodel.DataAnnotations;

namespace WebMonk.RazorSharp.HtmlTags.BaseTags
{
    public class SelfClosingInlineTag : SelfClosingTag
    {
        #region Constructors
        public SelfClosingInlineTag(string name, object? attributes, bool generateInline) : base(name, attributes) 
        { 
            GenerateInline = generateInline;
        }
        #endregion

        #region Overrides
        public override StringBuilderWithIndents ToHtml(StringBuilderWithIndents? sb = null)
        {
            if (GenerateInline)
            {
                sb ??= new StringBuilderWithIndents();
                sb.TrimEndWhitespace();
                sb.Append($"<{Name}{GenerateMyAttributesString()} />");
                return sb;
            }
            else
            {
                return base.ToHtml(sb);
            }
        }
        #endregion

        #region Properies
        public bool GenerateInline { get; }
        #endregion
    }
}