#nullable enable

using System;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.DataAnnotations.Misc;
using Supermodel.Presentation.Cmd.Models.Base;

namespace Supermodel.Presentation.Cmd.Models
{
        public class CheckboxCmdModel : UIComponentBase
        {
            #region IRMapperCustom implemtation
            public override Task MapFromCustomAsync<T>(T other)
            {
                if (typeof(T) != typeof(bool) && typeof(T) != typeof(bool?)) throw new ArgumentException("other must be of bool type", nameof(other));

                Value = (other != null ? other.ToString() : false.ToString())!;
                return Task.CompletedTask;
            }
            // ReSharper disable once RedundantAssignment
            public override Task<T> MapToCustomAsync<T>(T other)
            {
                if (typeof(T) != typeof(bool) && typeof(T) != typeof(bool?)) throw new ArgumentException("other must be of bool type", nameof(other));

                other = (T)(object)bool.Parse(Value);
                return Task.FromResult(other);
            }
            #endregion

            #region ICmdEditor
            public override IGenerateHtml EditorTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
            {
                var htmlAttributes = new AttributesDict(HtmlAttributesAsDict);
                htmlAttributes.Add("type", Type);
                htmlAttributes.Add("class", "form-check-input");

                var div = new HtmlStack();
                div.AppendAndPush(new Div(new { @class="form-check py-2" }));

                var checkBoxTags = div.Append(Render.CheckBoxForModel(ValueBool, htmlAttributes));
                var checkboxInputTag = (Input)checkBoxTags[0];
                if (checkboxInputTag.Attributes["type"] != "checkbox") throw new SupermodelException("checkboxInputTag.Attributes[\"type\"] != \"checkbox\": this should never happen");
                checkboxInputTag.AddOrUpdateAttr(attributes);
                
                div.Pop<Div>();
                return div;
            }
            #endregion

            #region ICmdDisplayer
            public override IGenerateHtml DisplayTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
            {
                return new Txt(ValueBool.ToYesNo());
            }
            #endregion

            #region IUIComponentWithValue implemetation
            public override string ComponentValue 
            {
                get => Value;
                set => Value = value;
            }
            #endregion

            #region Properies
            public string Value { get; set; } = false.ToString();
            public bool ValueBool
            {
                get
                {
                    if (string.IsNullOrEmpty(Value)) return false;
                    if (bool.TryParse(Value, out var boolean)) return boolean;
                    return false;
                }
                set => Value = value.ToString();
            }
            #endregion
        }
}
