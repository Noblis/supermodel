#nullable enable

using System;
using Supermodel.DataAnnotations;

namespace WebMonk.RazorSharp.HtmlTags.BaseTags
{
    public class SelfClosingTag : Tag
    {
        #region Constructors
        public SelfClosingTag(string? name, object? attributes = null) : base(name, attributes) { }
        #endregion

        #region Overrides
        public override StringBuilderWithIndents ToHtml(StringBuilderWithIndents? sb = null)
        {
            sb ??= new StringBuilderWithIndents();
            sb.AppendLine($"<{Name}{GenerateMyAttributesString()} />");
            return sb;
        }
        // ReSharper disable once UnusedParameter.Global
        public new void Add(IGenerateHtml item)
        {
            throw new InvalidOperationException("Self-closing tags cannot contain other tags");
        }
        #endregion
    }
}
