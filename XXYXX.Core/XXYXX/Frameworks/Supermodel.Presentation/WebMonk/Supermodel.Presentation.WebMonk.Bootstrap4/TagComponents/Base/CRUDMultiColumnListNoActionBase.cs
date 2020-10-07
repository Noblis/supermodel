﻿#nullable enable

using System.Collections.Generic;
using System.Linq;
using Supermodel.Presentation.WebMonk.Bootstrap4.Extensions;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Models.Mvc;
using Supermodel.ReflectionMapper;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;

namespace Supermodel.Presentation.WebMonk.Bootstrap4.TagComponents.Base
{
    public abstract class CRUDMultiColumnListNoActionBase : HtmlSnippet
    { 
        #region Constructors
        protected CRUDMultiColumnListNoActionBase(IEnumerable<IMvcModelForEntity> items, IGenerateHtml? pageTitle)
        {
            if (pageTitle != null) Append(new H2(new { @class = Bs4.ScaffoldingSettings.ListTitleCssClass }) { pageTitle });
            
            AppendAndPush(new Div(new { id = Bs4.ScaffoldingSettings.CRUDListTopDivId, @class = Bs4.ScaffoldingSettings.CRUDListTopDivCssClass }));
            AppendAndPush(new Table(new { id = Bs4.ScaffoldingSettings.CRUDListTableId, @class = Bs4.ScaffoldingSettings.CRUDListTableCssClass }));
            
            AppendAndPush(new Thead());
            AppendAndPush(new Tr());
            //Create header using reflection
            var mvcModelType = items.GetType().GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)).Select(t => t.GetGenericArguments()[0]).First();
            var mvcModelForHeader = ReflectionHelper.CreateType(mvcModelType);
            Append(mvcModelForHeader.ToReadOnlyHtmlTableHeader());
            Pop<Tr>();
            Pop<Thead>();

            AppendAndPush(new Tbody());
            foreach (var item in items)
            {
                AppendAndPush(new Tr());

                //Render list columns using reflection
                Append(item.ToReadOnlyHtmlTableRow());

                Pop<Tr>();
            }
            Pop<Tbody>();

            Pop<Table>();
            Pop<Div>();
        }
        #endregion
    }
}
