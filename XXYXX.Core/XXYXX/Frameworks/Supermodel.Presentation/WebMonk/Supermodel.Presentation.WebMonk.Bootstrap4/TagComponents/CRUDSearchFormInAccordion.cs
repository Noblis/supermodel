#nullable enable

using System;
using System.Collections.Generic;
using Supermodel.Presentation.WebMonk.Bootstrap4.TagComponents.Base;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Templates;

// ReSharper disable once CheckNamespace
namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    { 
        public class CRUDSearchFormInAccordion : AccordionBase
        {
            #region Constructors
            public CRUDSearchFormInAccordion(IEditorTemplate searchModel, string accordionId, IEnumerable<AccordionPanel> panels, string pageTitle, string? action = null, string? controller = null, bool resetButton = false) :
                this(searchModel, accordionId, panels, new Txt(pageTitle), controller, action, resetButton)
            { }

            public CRUDSearchFormInAccordion(IEditorTemplate searchModel, string accordionId, IEnumerable<AccordionPanel> panels, IGenerateHtml? pageTitle = null, string? action = null, string? controller = null, bool resetButton = false)
            {
                if (searchModel == null) throw new ArgumentException(nameof(searchModel));
            
                AppendAndPush(new CRUDSearchFormContainer(pageTitle, action, controller, resetButton));
                AppendAndPush(new Div(new { @class="accordion", id=accordionId }));
            
                foreach (var panel in panels)
                {
                    var body = searchModel.EditorTemplate(panel.ScreenOrderFrom, panel.ScreenOrderTo);
                    Append(GetAccordionSection(accordionId, panel, body));
                }

                Pop<Div>();
                Pop<CRUDSearchFormContainer>();
            }
            #endregion
        }
    }
}
