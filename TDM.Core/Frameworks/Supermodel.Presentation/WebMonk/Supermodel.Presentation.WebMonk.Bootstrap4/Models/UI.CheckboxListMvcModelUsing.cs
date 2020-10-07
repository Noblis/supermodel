#nullable enable

using System.Linq;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models.Base;
using WebMonk.Context;
using WebMonk.Extensions;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class CheckboxListMvcModelUsing<TMvcModel> : MultiSelectMvcModelUsing<TMvcModel> where TMvcModel : MvcModelForEntityCore
        {
            #region IEditorTemplate implementation
            public override IGenerateHtml EditorTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
            {
                var prefix = HttpContext.Current.PrefixManager.CurrentPrefix;
                var id = prefix.ToHtmlId();
                var name = id.ToHtmlName();
                
                var html = new HtmlStack();
            
                if (Orientation == Orientation.Vertical)
                {
                    var i = 1;
                    foreach (var option in Options.Where(x => x.IsShown))
                    {
                        var label = GetFullLabel(option);
                        
                        html.AppendAndPush(new Div(new { @class="form-check"}));
                        
                        var input = html.Append(new Input(new { @class="form-check-input", type="checkbox", value=option.Value, id=$"{id}{i}", name }));
                        input.AddOrUpdateAttr(attributes);
                        if (option.Selected) input.Attributes.Add("checked", "on");

                        html.Append(new Label(new { @class="form-check-label", @for=$"{id}{i}"}) { new Txt(label) });
                        
                        html.Pop<Div>();
                        i++;
                    }
                }
                else
                {
                    var i = 1;
                    foreach (var option in Options.Where(x => x.IsShown))
                    {
                        var label = GetFullLabel(option);

                        html.AppendAndPush(new Div(new { @class="form-check form-check-inline" }));

                        var input = html.Append(new Input(new { @class="form-check-input", type="checkbox", value=option.Value, id=$"{id}{i}", name }));
                        input.AddOrUpdateAttr(attributes);
                        if (option.Selected) input.Attributes.Add("checked", "on");

                        html.Append(new Label(new { @class="form-check-label", @for=$"{id}{i}"}) { new Txt(label) });

                        html.Pop<Div>();
                        i++;
                    }
                }
                
                html.Append(Render.HiddenForModel(""));
                return html;
            }
            #endregion

            #region Properties
            public Orientation Orientation { get; set; } = Orientation.Vertical;
            #endregion
        }
    }
}
