#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Supermodel.DataAnnotations;
using Supermodel.DataAnnotations.Validations.Attributes;
using Supermodel.Presentation.Mvc.Extensions;
using Supermodel.Presentation.Mvc.HtmlHelpers;
using Supermodel.Presentation.Mvc.Models.Mvc;
using Supermodel.ReflectionMapper;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class MvcModel : IMvcModel, IAsyncInit
        {
            #region EmbeddedTypes
            public enum NumberOfColumnsEnum
            {
                One = 1, 
                Two = 2, 
                Three = 3,
                Four = 4,
                Six = 6,
                Twelve = 12
            }
            #endregion
            
            #region IAsyncInit implementation
            [ScaffoldColumn(false), NotRMapped] public virtual bool AsyncInitialized { get; protected set; }
            public async Task InitAsync()
            {
                //If already initialized, do nothing
                if (AsyncInitialized) return;

                //Run init async for all properties that we will show
                foreach (var propertyInfo in GetType().GetProperties())
                {
                    var typedModel = this.PropertyGet(propertyInfo.Name);
                    if (typedModel is IAsyncInit iAsyncInitModel && !iAsyncInitModel.AsyncInitialized) 
                    {
                        await iAsyncInitModel.InitAsync();
                    }
                }

                //Mark as initialized
                AsyncInitialized = true;
            }
            #endregion
            
            #region Methods
            public virtual IHtmlContent EditorTemplate<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, string? markerAttribute = null)
            {
                if (NumberOfColumns != NumberOfColumnsEnum.One) return EditorTemplateForMultipleColumnsInternal(html, screenOrderFrom, screenOrderTo, markerAttribute, NumberOfColumns);

                if (html.ViewData.Model == null) throw new NullReferenceException(ReflectionHelper.GetCurrentContext() + " is called for a model that is null");
                if (!(html.ViewData.Model is IMvcModel)) throw new InvalidCastException(ReflectionHelper.GetCurrentContext() + " is called for a model of type different from MvcModel.");

                var result = new StringBuilder();
                var showValidationSummary = !html.ViewData.ModelState.IsValid;
                foreach (var propertyInfo in GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
                {
                    //Div 1
                    var propMarkerAttribute = markerAttribute;
                    var htmlAttrAttribute = propertyInfo.GetAttribute<HtmlAttrAttribute>();
                    if (htmlAttrAttribute != null) propMarkerAttribute += " " + htmlAttrAttribute.Attr;
                    result.AppendLine("<div class='form-group row' " + propMarkerAttribute + " >"); 

                    //Label
                    var hideLabelAttribute = propertyInfo.GetAttribute<HideLabelAttribute>();
                    if (hideLabelAttribute == null)
                    {
                        var labelHtml = html.Super().Label(propertyInfo.Name, new { @class = ScaffoldingSettings.EditorLabelCssClass }).GetString();
                        if (!propertyInfo.HasAttribute<NoRequiredLabelAttribute>())
                        {
                            if (propertyInfo.HasAttribute<RequiredAttribute>() || propertyInfo.HasAttribute<ForceRequiredLabelAttribute>()) labelHtml = labelHtml.Replace("</label>", $"<sup><em class='text-danger font-weight-bold {ScaffoldingSettings.RequiredAsteriskCssClass}'>*</em></sup></label>", true, CultureInfo.InvariantCulture);
                        }
                        result.AppendLine(labelHtml);
                    }
                    else
                    {
                        if (hideLabelAttribute.KeepLabelSpace) result.AppendLine($"<div {UtilsLib.MakeClassAttribute(ScaffoldingSettings.EditorLabelCssClass)}></div>");
                    }
                    
                    //Div 2
                    if (hideLabelAttribute == null || hideLabelAttribute.KeepLabelSpace) result.AppendLine("<div class='col-sm-10'>");
                    else result.AppendLine("<div class='col-sm-12'>");

                    //Value
                    if (!propertyInfo.HasAttribute<DisplayOnlyAttribute>())
                    {
                        var controlHtml = html.Super().Editor(propertyInfo.Name).GetString();
                        //we do not show validation errors in a model if SelectedId is set. It means that these are inline edit errors
                        //if (!html.ViewData.ModelState.IsValid && (long?)html.ViewBag.SelectedId == null)
                        if (!html.ViewData.ModelState.IsValid)
                        {
                            result.AppendLine(controlHtml);
                            
                            var msg = html.ValidationMessage(propertyInfo.Name, null, new { @class=ScaffoldingSettings.ValidationErrorCssClass }).GetString();
                            if (!msg.Contains("></span>")) showValidationSummary = false;
                            msg = msg.Replace("<span ", "<div ").Replace("</span>", "</div>");
                            result.Append(msg);

                            //var value = html.ViewData.ModelState.SingleOrDefault(x => x.Key == propertyInfo.Name).Value;
                            //if (value?.Errors != null && value.Errors.Any(x => !string.IsNullOrEmpty(x.ErrorMessage)))
                            //{
                            //    result.Append($"<div class='{Bs4.ScaffoldingSettings.ValidationErrorCssClass}'>");
                            //    result.AppendLine(value.Errors.First(x => !string.IsNullOrEmpty(x.ErrorMessage)).ErrorMessage);
                            //    result.Append("</div>");
                            //}
                        }
                        else
                        {
                            result.AppendLine(controlHtml);
                        }
                    }
                    else
                    {
                        result.AppendLine("<span " + UtilsLib.MakeClassAttribute(ScaffoldingSettings.DisplayCssClass) + ">");
                        result.AppendLine(html.Super().Display(propertyInfo.Name).GetString());
                        result.AppendLine("</span>");
                    }
                    
                    result.AppendLine("</div>"); //close Div 2
                    if (showValidationSummary)
                    {
                        result.AppendLine($"<div class='col-sm-12 {ScaffoldingSettings.ValidationSummaryCssClass}'>");
                        result.AppendLine(html.ValidationSummary().GetString());
                        result.AppendLine("</div>");
                    }
                    result.AppendLine("</div>"); //close Div 1
                }
                return result.ToHtmlString(); 
            }
            public virtual IHtmlContent DisplayTemplate<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, string? markerAttribute = null)
            {
                if (NumberOfColumns != NumberOfColumnsEnum.One) return DisplayTemplateForMultipleColumnsInternal(html, screenOrderFrom, screenOrderTo, markerAttribute, NumberOfColumns);
                
                if (html.ViewData.Model == null) throw new NullReferenceException(ReflectionHelper.GetCurrentContext() + " is called for a model that is null");
                if (!(html.ViewData.Model is IMvcModel)) throw new InvalidCastException(ReflectionHelper.GetCurrentContext() + " is called for a model of type different from MvcModel.");

                var result = new StringBuilder();
                foreach (var propertyInfo in GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
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
            public virtual IHtmlContent HiddenTemplate<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, string? markerAttribute = null)
            {
                if (html.ViewData.Model == null) throw new NullReferenceException(ReflectionHelper.GetCurrentContext() + " is called for a model that is null");
                if (!(html.ViewData.Model is IMvcModel)) throw new InvalidCastException(ReflectionHelper.GetCurrentContext() + " is called for a model of type different from MvcModel.");

                var result = new StringBuilder();
                foreach (var propertyInfo in GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
                {
                    var hiddenFieldHtml = html.Super().Hidden(propertyInfo.Name).GetString();

                    var propMarkerAttribute = markerAttribute;
                    var htmlAttrAttribute = propertyInfo.GetAttribute<HtmlAttrAttribute>();
                    if (htmlAttrAttribute != null) propMarkerAttribute += " " + htmlAttrAttribute.Attr;
                    if (!string.IsNullOrEmpty(propMarkerAttribute)) hiddenFieldHtml = hiddenFieldHtml.Replace(">", $" {propMarkerAttribute}>");

                    result.AppendLine(hiddenFieldHtml);
                }
                return result.ToHtmlString(); 
            }
            
            protected virtual IHtmlContent EditorTemplateForMultipleColumnsInternal<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom, int screenOrderTo, string? markerAttribute, NumberOfColumnsEnum numberOfColumns)
            {
                if (html.ViewData.Model == null) throw new NullReferenceException(ReflectionHelper.GetCurrentContext() + " is called for a model that is null");
                if (!(html.ViewData.Model is IMvcModel)) throw new InvalidCastException(ReflectionHelper.GetCurrentContext() + " is called for a model of type different from MvcModel.");

                var maxColumns = (int)numberOfColumns;
                var currentColumn = 1;
                var columnSpanClass = $"class=\"form-group col-md-{12/maxColumns}\"";
                
                var result = new StringBuilder();
                var showValidationSummary = !html.ViewData.ModelState.IsValid;
                foreach (var propertyInfo in GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
                {
                    //If this is a beginning of a row
                    if (currentColumn == 1) result.AppendLine("<div class='form-row'>"); 

                    //Div
                    var propMarkerAttribute = markerAttribute;
                    var htmlAttrAttribute = propertyInfo.GetAttribute<HtmlAttrAttribute>();
                    if (htmlAttrAttribute != null) propMarkerAttribute += " " + htmlAttrAttribute.Attr;
                    
                    result.AppendLine($"<div {columnSpanClass} {propMarkerAttribute} >");

                    //Label
                    var hideLabelAttribute = propertyInfo.GetAttribute<HideLabelAttribute>();
                    if (hideLabelAttribute == null)
                    {
                        var labelHtml = html.Super().Label(propertyInfo.Name, new { @class = ScaffoldingSettings.EditorMultiColumnLabelCssClass }).GetString();
                        if (!propertyInfo.HasAttribute<NoRequiredLabelAttribute>())
                        {
                            if (propertyInfo.HasAttribute<RequiredAttribute>() || propertyInfo.HasAttribute<ForceRequiredLabelAttribute>()) labelHtml = labelHtml.Replace("</label>", $"<sup><em class='text-danger font-weight-bold {ScaffoldingSettings.RequiredAsteriskCssClass}'>*</em></sup></label>", true, CultureInfo.InvariantCulture);
                        }
                        result.AppendLine(labelHtml);
                    }
                    else
                    {
                        if (hideLabelAttribute.KeepLabelSpace) result.AppendLine($"<div {UtilsLib.MakeClassAttribute(ScaffoldingSettings.EditorMultiColumnLabelCssClass)}></div>");
                    }

                    //Value
                    if (!propertyInfo.HasAttribute<DisplayOnlyAttribute>())
                    {
                        result.AppendLine(html.Super().Editor(propertyInfo.Name).GetString());
                        
                        //we do not show validation errors in a model if SelectedId is set. It means that these are inline edit errors
                        //if (!html.ViewData.ModelState.IsValid && (long?)html.ViewBag.SelectedId == null) 
                        if (!html.ViewData.ModelState.IsValid)
                        {
                            var msg = html.ValidationMessage(propertyInfo.Name, null, new { @class=ScaffoldingSettings.ValidationErrorCssClass }).GetString();
                            if (!msg.Contains("></span>")) showValidationSummary = false;
                            msg = msg.Replace("<span ", "<div ").Replace("</span>", "</div>");
                            result.Append(msg);
                            
                            //var value = html.ViewData.ModelState.SingleOrDefault(x => x.Key == propertyInfo.Name).Value;
                            //if (value?.Errors != null && value.Errors.Any(x => !string.IsNullOrEmpty(x.ErrorMessage)))
                            //{
                            //    result.Append($"<div class='{Bs4.ScaffoldingSettings.ValidationErrorCssClass}'>");
                            //    result.AppendLine(value.Errors.First(x => !string.IsNullOrEmpty(x.ErrorMessage)).ErrorMessage);
                            //    result.Append("</div>");
                            //}
                        }
                    }
                    else
                    {
                        result.AppendLine("<div " + UtilsLib.MakeClassAttribute(ScaffoldingSettings.MultiColumnDisplayCssClass) + ">");
                        result.AppendLine(html.Super().Display(propertyInfo.Name).GetString());
                        result.AppendLine("</div>");
                    }                    
                    result.AppendLine("</div>"); //close Div

                    //if this is an ending of a row
                    if (currentColumn == maxColumns)
                    {
                        currentColumn = 1;
                        result.AppendLine("</div>");
                    }
                    else
                    {
                        currentColumn++;
                    }
                }

                if (showValidationSummary)
                {
                    result.AppendLine($"<div class='col-sm-12 {ScaffoldingSettings.ValidationSummaryCssClass}'>");
                    result.AppendLine(html.ValidationSummary().GetString());
                    result.AppendLine("</div>");
                }

                if (currentColumn != 1) result.AppendLine("</div>");

                return result.ToHtmlString();                 
            }
            protected virtual IHtmlContent DisplayTemplateForMultipleColumnsInternal<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom, int screenOrderTo, string? markerAttribute, NumberOfColumnsEnum numberOfColumns)
            {
                if (html.ViewData.Model == null) throw new NullReferenceException(ReflectionHelper.GetCurrentContext() + " is called for a model that is null");
                if (!(html.ViewData.Model is IMvcModel)) throw new InvalidCastException(ReflectionHelper.GetCurrentContext() + " is called for a model of type different from MvcModel.");

                var maxColumns = (int)numberOfColumns;
                var currentColumn = 1;
                var columnSpanClass = $"class=\"form-group col-md-{12/maxColumns}\"";
                
                var result = new StringBuilder();
                foreach (var propertyInfo in GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo))
                {
                    //If this is a beginning of a row
                    if (currentColumn == 1) result.AppendLine("<div class='form-row'>"); 

                    //Div
                    var propMarkerAttribute = markerAttribute;
                    var htmlAttrAttribute = propertyInfo.GetAttribute<HtmlAttrAttribute>();
                    if (htmlAttrAttribute != null) propMarkerAttribute += " " + htmlAttrAttribute.Attr;
                    
                    result.AppendLine($"<div {columnSpanClass} {propMarkerAttribute} >");

                    //Label
                    var hideLabelAttribute = propertyInfo.GetAttribute<HideLabelAttribute>();
                    if (hideLabelAttribute == null)
                    {
                        var labelHtml = html.Super().Label(propertyInfo.Name, new { @class = ScaffoldingSettings.DisplayMultiColumnLabelCssClass }).GetString();
                        if (!propertyInfo.HasAttribute<NoRequiredLabelAttribute>())
                        {
                            if (propertyInfo.HasAttribute<RequiredAttribute>() || propertyInfo.HasAttribute<ForceRequiredLabelAttribute>()) labelHtml = labelHtml.Replace("</label>", $"<sup><em class='text-danger font-weight-bold {ScaffoldingSettings.RequiredAsteriskCssClass}'>*</em></sup></label>", true, CultureInfo.InvariantCulture);
                        }
                        result.AppendLine(labelHtml);
                    }
                    else
                    {
                        if (hideLabelAttribute.KeepLabelSpace) result.AppendLine($"<div {UtilsLib.MakeClassAttribute(ScaffoldingSettings.DisplayMultiColumnLabelCssClass)}></div>");
                    }

                    //Value
                    result.AppendLine("<div " + UtilsLib.MakeClassAttribute(ScaffoldingSettings.MultiColumnDisplayCssClass) + ">");
                    result.AppendLine(html.Super().Display(propertyInfo.Name).GetString());
                    result.AppendLine("</div>");
                    
                    result.AppendLine("</div>"); //close Div

                    //if this is an ending of a row
                    if (currentColumn == maxColumns)
                    {
                        currentColumn = 1;
                        result.AppendLine("</div>");
                    }
                    else
                    {
                        currentColumn++;
                    }
                }
                if (currentColumn != 1) result.AppendLine("</div>");
                
                return result.ToHtmlString();                 
            }

            protected virtual IEnumerable<PropertyInfo> GetDetailPropertyInfosInOrder(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
            {
                return GetType().GetDetailPropertyInfosInOrder(screenOrderFrom, screenOrderTo);
            }
            #endregion

            #region Properties
            [ScaffoldColumn(false), NotRMapped] public virtual NumberOfColumnsEnum NumberOfColumns => NumberOfColumnsEnum.One;
            #endregion
        }
    }
}
