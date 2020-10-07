﻿#nullable enable

using Supermodel.DataAnnotations;

namespace WebMonk.RazorSharp.HtmlTags.BaseTags
{
    public class InlineTag : Tag
    {
        #region Constructors
        public InlineTag(string? name, object? attributes, bool generateInline) : base(name, attributes) 
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

                if (ContainsInnerHtml())
                {
                    sb.TrimEndWhitespace();
                    sb.Append($"<{Name}{GenerateMyAttributesString()}>");

                    foreach (var tag in this) sb = tag.ToHtml(sb);
                    
                    sb.TrimEndWhitespace();
                    sb.Append($"</{Name}>");
                }
                else
                {
                    sb.TrimEndWhitespace();
                    sb.Append($"<{Name}{GenerateMyAttributesString()}></{Name}>");
                }

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
