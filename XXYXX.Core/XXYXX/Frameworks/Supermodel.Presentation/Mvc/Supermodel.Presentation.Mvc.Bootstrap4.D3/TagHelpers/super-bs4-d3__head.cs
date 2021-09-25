﻿#nullable enable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Supermodel.Presentation.Mvc.Bootstrap4.TagHelpers;

namespace Supermodel.Presentation.Mvc.Bootstrap4.D3.TagHelpers
{
    [HtmlTargetElement("head", Attributes = "super-bs4-d3-add-meta-and-links")]
    public class SuperBs4D3HeadTagHelper : SuperBs4HeadTagHelper
    {
        #region Constructors
        public SuperBs4D3HeadTagHelper(IHtmlHelper<dynamic> htmlHelper) : base(htmlHelper) { }
        #endregion

        #region Methods
        public override void RemoveMarkerAttribute(TagHelperOutput output)
        {
            output.Attributes.Remove(new TagHelperAttribute("super-bs4-d3-add-meta-and-links"));
        }
        public override string GetSupermodelSnippet(IUrlHelper urlHelper)
        {
            return GetSupermodelSnippetStatic(urlHelper);
        }
        public new static string GetSupermodelSnippetStatic(IUrlHelper urlHelper)
        {
            // ReSharper disable Html.PathError
            var result = $@"
                <meta charset=""utf-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
                <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"" integrity=""sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm"" crossorigin=""anonymous"" />
                <link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/open-iconic/1.1.1/font/css/open-iconic-bootstrap.min.css"" integrity=""sha256-BJ/G+e+y7bQdrYkS2RBTyNfBHpA9IuGaPmf9htub5MQ="" crossorigin=""anonymous"" />
                <link rel=""stylesheet"" href=""https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"" />
                <link rel=""stylesheet"" href=""{urlHelper.Content("~/static_web_files/britecharts.min.css")}"" />
                <link rel=""stylesheet"" href=""{urlHelper.Content("~/static_web_files/super.bs4.css")}"" />
            ";
            // ReSharper restore Html.PathError

            //britecharts.min.css: https://cdn.jsdelivr.net/npm/britecharts/dist/css/britecharts.min.css
            return result;
        }
        #endregion

    }
}
