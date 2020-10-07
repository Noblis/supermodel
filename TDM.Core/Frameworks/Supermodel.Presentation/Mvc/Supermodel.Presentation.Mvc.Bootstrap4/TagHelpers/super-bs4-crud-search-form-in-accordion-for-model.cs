﻿#nullable enable

using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Bootstrap4.SuperHtmlHelpers;
using Supermodel.Presentation.Mvc.Bootstrap4.TagHelpers.Base;
using Supermodel.Presentation.Mvc.HtmlHelpers;

namespace Supermodel.Presentation.Mvc.Bootstrap4.TagHelpers
{
    [HtmlTargetElement("super-bs4-crud-search-form-in-accordion-for-model", Attributes = "panels", TagStructure = TagStructure.WithoutEndTag)]
    public class SuperBs4CRUDSearchFormInAccordionForModelTagHelper : TagHelperDerivedFromHtmlHelperBase
    {
        #region Constructors
        public SuperBs4CRUDSearchFormInAccordionForModelTagHelper(IHtmlHelper<dynamic> htmlHelper) : base(htmlHelper){ }
        #endregion

        #region Overrides
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            if (PageTitle != null) output.Content.SetHtmlContent(_htmlHelper.Super().Bs4().CRUDSearchFormInAccordionForModel(AccordionId, Panels!, PageTitle, Action, Controller, ResetButton));
            else output.Content.SetHtmlContent(_htmlHelper.Super().Bs4().CRUDSearchFormInAccordionForModel(AccordionId, Panels!, (IHtmlContent?)null, Action, Controller, ResetButton));
        }
        #endregion

        #region Properties
        [HtmlAttributeName("Id")] public string AccordionId { get; set; } = "accordion";
        public IEnumerable<Bs4.AccordionPanel>? Panels { get; set; }

        public string? PageTitle { get; set; } = null;
        public string? Controller {get; set; } = null;
        public string? Action { get; set; } = null;
        public bool ResetButton { get; set; } = false;
        #endregion
    }
}
