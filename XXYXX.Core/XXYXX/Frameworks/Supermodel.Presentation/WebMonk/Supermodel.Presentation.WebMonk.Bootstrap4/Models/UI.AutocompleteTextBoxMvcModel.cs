#nullable enable

using System;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.DataAnnotations.Misc;
using WebMonk.Extensions;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class AutocompleteTextBoxMvcModel : TextBoxMvcModel
        {
            #region Constructors
            public AutocompleteTextBoxMvcModel(){}
            public AutocompleteTextBoxMvcModel(Type autocompleteControllerType)
            {
                AutocompleteControllerName = autocompleteControllerType.GetApiControllerName();
            }
            public AutocompleteTextBoxMvcModel(string autocompleteControllerName)
            {
                AutocompleteControllerName = autocompleteControllerName;
            }
            #endregion

            #region Overrides

            public override IGenerateHtml EditorTemplate(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue, object? attributes = null)
            {
                if (AutocompleteControllerName == null) throw new SupermodelException("AutocompleteControllerType == null");

                var tmpHtmlAttributesAsDict = new AttributesDict(HtmlAttributesAsDict);
                
                HtmlAttributesAsDict["data-autocomplete-source"] = Render.Helper.UrlToApiAction(AutocompleteControllerName, "");
                var result = base.EditorTemplate(screenOrderFrom, screenOrderTo, attributes);
                
                HtmlAttributesAsDict = tmpHtmlAttributesAsDict;
                return result;
            }
            #endregion

            #region Properies
            public string? AutocompleteControllerName { get; set; } 
            #endregion
        }
    }
}
