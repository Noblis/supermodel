#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Models.Interfaces;

namespace Supermodel.Presentation.Cmd.Models
{
    public class CmdModel : ICmdModel
    {
        public virtual IConsoleOutput EditorTemplate<TModel>(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public virtual IConsoleOutput DisplayTemplate<TModel>(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            var result = new StringBuilderWithColor();
            foreach (var propertyInfo in GetType().GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
            {
                //Div 1
                var propMarkerAttribute = markerAttribute;
                var htmlAttrAttribute = propertyInfo.GetAttribute<HtmlAttrAttribute>();
                if (htmlAttrAttribute != null) propMarkerAttribute += " " + htmlAttrAttribute.Attr;
                result.AppendLine("<div class='form-group row'" + propMarkerAttribute + " >"); 

                //Label
                var hideLabelAttribute = propertyInfo.GetAttribute<HideLabelAttribute>();
                if (hideLabelAttribute == null)
                {
                    var labelHtml = html.Super().Label(propertyInfo.Name, new { @class = ScaffoldingSettings.DisplayLabelCssClass }).GetString();
                    if (!propertyInfo.HasAttribute<NoRequiredLabelAttribute>())
                    {
                        if (propertyInfo.HasAttribute<RequiredAttribute>() || propertyInfo.HasAttribute<ForceRequiredLabelAttribute>()) labelHtml = labelHtml.Replace("</label>", $"<sup><em class='text-danger font-weight-bold {ScaffoldingSettings.RequiredAsteriskCssClass}'>*</em></sup></label>", true, CultureInfo.InvariantCulture);
                    }
                    result.AppendLine(labelHtml);
                }
                else
                {
                    if (hideLabelAttribute.KeepLabelSpace) result.AppendLine($"<div {UtilsLib.MakeClassAttribute(ScaffoldingSettings.DisplayLabelCssClass)}></div>");
                }
                
                //Div 2
                if (hideLabelAttribute == null || hideLabelAttribute.KeepLabelSpace) result.AppendLine("<div class='col-sm-10'>");
                else result.AppendLine("<div class='col-sm-12'>");

                //Value
                result.AppendLine("<span " + UtilsLib.MakeClassAttribute(ScaffoldingSettings.DisplayCssClass) + ">");
                result.AppendLine(html.Super().Display(propertyInfo.Name).GetString());
                result.AppendLine("</span>");
                
                result.AppendLine("</div>"); //close Div 2
                result.AppendLine("</div>"); //close Div 1
            }
            return result.ToHtmlString();         
        }

        private IEnumerable<PropertyInfo> GetDetailPropertyInfosInOrder(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            var result = GetType().GetProperties()
                .Where(x => x.GetCustomAttribute<ScaffoldColumnAttribute>() == null || x.GetCustomAttribute<ScaffoldColumnAttribute>()!.Scaffold)
                .Where(x => (x.GetCustomAttribute<ScreenOrderAttribute>() != null ? x.GetCustomAttribute<ScreenOrderAttribute>()!.Order : 100) >= screenOrderFrom)
                .Where(x => (x.GetCustomAttribute<ScreenOrderAttribute>() != null ? x.GetCustomAttribute<ScreenOrderAttribute>()!.Order : 100) <= screenOrderTo)
                .OrderBy(x => x.GetCustomAttribute<ScreenOrderAttribute>() != null ? x.GetCustomAttribute<ScreenOrderAttribute>()!.Order : 100);
        
            //By default we do not scaffold enumerations (except for strings of course and unless they implement ISupermodelEditorTemplate)
            return result.Where(x => !(x.PropertyType != typeof(string) && 
                                       !typeof(ISupermodelEditorTemplate).IsAssignableFrom(x.PropertyType) && 
                                       typeof(IEnumerable).IsAssignableFrom(x.PropertyType)));
        }

    }
}
