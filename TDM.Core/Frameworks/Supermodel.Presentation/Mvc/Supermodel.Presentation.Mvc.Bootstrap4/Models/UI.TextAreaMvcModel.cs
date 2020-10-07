#nullable enable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Supermodel.DataAnnotations.Misc;
using Supermodel.Presentation.Mvc.Extensions;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Models
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

            #region ISupermodelEditorTemplate implemtation
            public override IHtmlContent EditorTemplate<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, string? markerAttribute = null)
            {
                var htmlAttributes = new AttributesDict(HtmlAttributesAsDict);
                htmlAttributes.Add("type", Type);
                htmlAttributes.Add("rows", Rows.ToString());
                htmlAttributes.Add("class", "form-control");
                
                if (Pattern != "") htmlAttributes.Add("pattern", Pattern);
                if (Step != "") htmlAttributes.Add("step", Step);
                
                // ReSharper disable once ConstantNullCoalescingCondition
                var text = html.TextArea("", Value ?? "", htmlAttributes).GetString();
                text = text.Replace("/>", $"{markerAttribute} />");
                return text.ToHtmlString();
            }
            #endregion

            #region Properties
            public int Rows { get; set; } = 3;
            #endregion
        }
    }
}
