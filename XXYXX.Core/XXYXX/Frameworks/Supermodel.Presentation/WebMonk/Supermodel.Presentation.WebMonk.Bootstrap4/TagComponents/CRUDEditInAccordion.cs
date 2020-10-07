﻿#nullable enable

using System.Collections.Generic;
using Supermodel.Presentation.WebMonk.Bootstrap4.TagComponents.Base;
using Supermodel.Presentation.WebMonk.Models;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Templates;

// ReSharper disable once CheckNamespace
namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    {
        public class CRUDEditInAccordion : AccordionBase
        {
            #region Constructors
            public CRUDEditInAccordion(IEditorTemplate model, string accordionId, IEnumerable<AccordionPanel> panels, string pageTitle, bool readOnly = false, bool skipBackButton = false, bool skipHeaderAndFooter = false) :
                this(model, accordionId, panels, new Txt(pageTitle), readOnly, skipBackButton, skipHeaderAndFooter)
            { }

            public CRUDEditInAccordion(IEditorTemplate model, string accordionId, IEnumerable<AccordionPanel> panels, IGenerateHtml? pageTitle = null, bool readOnly = false, bool skipBackButton = false, bool skipHeaderAndFooter = false)
            {
                if (!skipHeaderAndFooter) AppendAndPush(new CRUDEditContainer((IViewModelForEntity)model, pageTitle, readOnly, skipBackButton));

                AppendAndPush(new Div(new { id=accordionId }));
                foreach (var panel in panels)
                {
                    var body = model.EditorTemplate(panel.ScreenOrderFrom, panel.ScreenOrderTo).DisableAllControlsIf(readOnly);
                    Append(GetAccordionSection(accordionId, panel, body));
                }
                Pop<Div>();

                if (!skipHeaderAndFooter) Pop<CRUDEditContainer>();
            }
            #endregion
        }
    }
}
