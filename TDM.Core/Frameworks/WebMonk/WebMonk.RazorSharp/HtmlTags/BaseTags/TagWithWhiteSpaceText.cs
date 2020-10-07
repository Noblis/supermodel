#nullable enable

using Supermodel.DataAnnotations;

namespace WebMonk.RazorSharp.HtmlTags.BaseTags
{
    public class TagWithWhiteSpaceText : Tag
    {
        #region Constructors
        public TagWithWhiteSpaceText(string? name, object? attributes = null) : base(name, attributes) { }
        #endregion

        #region Overrides
        public override StringBuilderWithIndents ToHtml(StringBuilderWithIndents? sb = null)
        {
            sb ??= new StringBuilderWithIndents();

            sb.Append($"<{Name}{GenerateMyAttributesString()}>");
            foreach (var tag in this) 
            {
                if (tag is Txt txtTag) txtTag.ToHtmlNoNewLineAtTheEnd(sb);
                else sb = tag.ToHtml(sb);
            }
            sb.AppendLine($"</{Name}>");

            return sb;
        }
        #endregion
    }
}

