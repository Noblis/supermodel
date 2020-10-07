#nullable enable

using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Supermodel.Presentation.Mvc.Bootstrap4.Models.Base;
using Supermodel.Presentation.Mvc.Extensions;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class CheckboxListMvcModelUsing<TMvcModel> : MultiSelectMvcModelUsing<TMvcModel> where TMvcModel : MvcModelForEntityCore
        {
            #region IEditorTemplate implemetation
            public override IHtmlContent EditorTemplate<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, string? markerAttribute = null)
            {
                var prefix = html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix;
                var name = prefix;
                var id = name.Replace(".", "_");
                
                var sb = new StringBuilder();

                if (Orientation == Orientation.Vertical)
                {
                    var i = 1;
                    foreach (var option in Options.Where(x => x.IsShown))
                    {
                        var label = GetFullLabel(option);
                        
                        sb.AppendLine("<div class='form-check'>");
                        
                        var input = $"<input class='form-check-input' type='checkbox' value='{option.Value}' id='{id}{i}' name='{name}' {markerAttribute} />";
                        if (option.Selected) input = input.Replace(" />", " checked='on' />");
                        sb.AppendLine(input);

                        sb.AppendLine($"<label class='form-check-label' for='{id}{i}'>{label}</label>");

                        sb.AppendLine("</div>");
                        i++;
                    }
                }
                else
                {
                    var i = 1;
                    foreach (var option in Options.Where(x => x.IsShown))
                    {
                        var label = GetFullLabel(option);

                        sb.AppendLine("<div class='form-check form-check-inline'>");

                        var input = $"<input class='form-check-input' type='checkbox' value='{option.Value}' id='{id}{i}' name='{name}' {markerAttribute} />";
                        if (option.Selected) input = input.Replace(" />", " checked='on' />");
                        sb.AppendLine(input);

                        sb.AppendLine($"<label class='form-check-label' for='{id}{i}'>{label}</label>");

                        sb.AppendLine("</div>");
                        i++;
                    }
                }

                sb.AppendLine(html.Hidden("", "", null).GetString());
                return sb.ToHtmlString();
            }
            #endregion

            #region Properties
            public Orientation Orientation { get; set; } = Orientation.Vertical;
            #endregion
        }        
    }
}
