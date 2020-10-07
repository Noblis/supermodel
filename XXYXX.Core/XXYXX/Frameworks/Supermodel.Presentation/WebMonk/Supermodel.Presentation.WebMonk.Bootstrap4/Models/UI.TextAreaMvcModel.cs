#nullable enable

using System;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Misc;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class TextAreaMvcModel : TextBoxMvcModel
        {
            #region Constructors
            public TextAreaMvcModel()
            {
                // ReSharper disable once VirtualMemberCallInConstructor
                InitFor<string>();
            }
            #endregion
            
            #region IRMapperCustom implemtation
            public override Task MapFromCustomAsync<T>(T other)
            {
                if (typeof(T) != typeof(string)) throw new ArgumentException("other must be of string type", nameof(other));
                InitFor<string>();
                Value = (other != null ? other.ToString() : "")!;
                return Task.CompletedTask;
            }
            // ReSharper disable once RedundantAssignment
            public override Task<T> MapToCustomAsync<T>(T other)
            {
                if (typeof(T) != typeof(string)) throw new ArgumentException("other must be of string type", nameof(other));

                other = (T)(object)Value;
                return Task.FromResult(other);
            }
            #endregion

            #region IEditorTemplate implementation
            public override IGenerateHtml EditorTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
            {
                var htmlAttributes = new AttributesDict(HtmlAttributesAsDict);
                htmlAttributes.Add("type", Type);
                htmlAttributes.Add("rows", Rows.ToString());
                htmlAttributes.Add("class", "form-control");
                
                if (Pattern != "") htmlAttributes.Add("pattern", Pattern);
                if (Step != "") htmlAttributes.Add("step", Step);
                
                return Render.TextAreaForModel(Value, htmlAttributes).AddOrUpdateAttr(attributes);
            }
            #endregion

            #region Properties
            public int Rows { get; set; } = 3;
            #endregion
        }
    }
}
