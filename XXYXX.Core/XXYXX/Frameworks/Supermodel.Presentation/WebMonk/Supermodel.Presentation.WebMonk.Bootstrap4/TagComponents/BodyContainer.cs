#nullable enable

using System.Web;
using Supermodel.Presentation.WebMonk.Extensions.Gateway;
using WebMonk.Context;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;

// ReSharper disable once CheckNamespace
namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    { 
        public class BodyContainer : HtmlContainerSnippet
        {
            #region Constructors
            public BodyContainer(object? bodyAttributes = null)
            {
                AppendAndPush(new Body(bodyAttributes)); 

                Append(new Script(new { src="https://code.jquery.com/jquery-3.4.1.min.js", integrity="sha256-CSXorXvZcTkaix6Yvo6HppcZGetbYMGWSFlBw8HfCJo=", crossorigin="anonymous" }));
                Append(new Script(new { src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js", integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q", crossorigin="anonymous" }));
                Append(new Script(new { src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js", integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM", crossorigin="anonymous" }));
                Append(new Script(new { src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js", integrity="sha256-VazP97ZCwtekAsvgPBSUwPFKdrwD3unUfSGVYrahUqU=", crossorigin="anonymous" }));
                Append(new Script(new { src="/js/bootbox.all.min.js"}));
                Append(new Script(new { src="/js/super.bs4.js" }));
                
                if (HttpContext.Current.TempData.Super().NextPageStartupScript != null ||
                    HttpContext.Current.TempData.Super().NextPageAlertMessage != null ||
                    HttpContext.Current.TempData.Super().NextPageModalMessage != null)
                {
                    AppendAndPush(new Script());
                    Append(new Txt("$(function () {"));

                    if (HttpContext.Current.TempData.Super().NextPageStartupScript != null) Append(new Txt(HttpContext.Current.TempData.Super().NextPageStartupScript ?? ""));
                    if (HttpContext.Current.TempData.Super().NextPageAlertMessage != null) Append(new Txt($"alert(\"{HttpUtility.HtmlEncode(HttpContext.Current.TempData.Super().NextPageAlertMessage!)}\");"));
                    if (HttpContext.Current.TempData.Super().NextPageModalMessage != null) Append(new Txt($"bootbox.alert(\"{HttpUtility.HtmlEncode(HttpContext.Current.TempData.Super().NextPageModalMessage!)}\".replace(/\\n/g, \"<br />\"));"));

                    Append(new Txt("});"));

                    Pop<Script>();
                }

                Append(InnerContent = new Tags());

                Pop<Body>();
            }
            #endregion
        }
    }
}
