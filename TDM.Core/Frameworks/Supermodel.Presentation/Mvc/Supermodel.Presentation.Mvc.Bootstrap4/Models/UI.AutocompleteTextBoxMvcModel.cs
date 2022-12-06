#nullable enable

using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.DataAnnotations.Misc;
using Supermodel.Presentation.Mvc.Extensions;
using Supermodel.Presentation.Mvc.HtmlHelpers;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class AutocompleteTextBoxMvcModel : TextBoxMvcModel
        {
            #region Constructors
            public AutocompleteTextBoxMvcModel(){}
            public AutocompleteTextBoxMvcModel(Type autocompleteControllerType)
            {
                AutocompleteControllerName = autocompleteControllerType.Name.RemoveControllerSuffix();
            }
            public AutocompleteTextBoxMvcModel(string autocompleteControllerName)
            {
                AutocompleteControllerName = autocompleteControllerName;
            }
            #endregion

            #region Overrides
            public override IHtmlContent EditorTemplate<TModel>(IHtmlHelper<TModel> html, int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, string? markerAttribute = null)
            {
                if (AutocompleteControllerName == null) throw new SupermodelException("AutocompleteControllerType == null");

                var tmpHtmlAttributesAsDict = new AttributesDict(HtmlAttributesAsDict);
                
                HtmlAttributesAsDict["data-autocomplete-source"] = html.Super().GenerateUrl("", AutocompleteControllerName);
                var result = base.EditorTemplate(html, screenOrderFrom, screenOrderTo, markerAttribute);
                
                HtmlAttributesAsDict = tmpHtmlAttributesAsDict;
                return result;
            }
            public override TextBoxMvcModel InitFor<T>()
            {
                return base.InitFor<string>(); //autocomplete is always a string text box
            }
            #endregion

            #region Properies
            public string? AutocompleteControllerName { get; set; } 
            #endregion
        }
    }
}
