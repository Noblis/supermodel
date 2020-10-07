#nullable enable

using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;

// ReSharper disable once CheckNamespace
namespace Supermodel.Presentation.WebMonk.Bootstrap4.Models
{
    public static partial class Bs4
    { 
        public class HeadContainer : HtmlContainerSnippet
        {
            #region Constructors
            public HeadContainer(object? headAttributes = null)
            {
                AppendAndPush(new Head(headAttributes));
                
                Append(new Meta(new { charset="utf-8"}));
                Append(new Meta(new { name="viewport", content="width=device-width, initial-scale=1, shrink-to-fit=no"}));

                Append(new Link(new { rel="stylesheet", href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css", integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm", crossorigin="anonymous"}));
                Append(new Link(new { rel="stylesheet", href="https://cdnjs.cloudflare.com/ajax/libs/open-iconic/1.1.1/font/css/open-iconic-bootstrap.min.css", integrity="sha256-BJ/G+e+y7bQdrYkS2RBTyNfBHpA9IuGaPmf9htub5MQ=", crossorigin="anonymous"}));
                Append(new Link(new { rel="stylesheet", href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" }));
                Append(new Link(new { rel="stylesheet", href="/css/super.bs4.css" }));

                Append(InnerContent = new Tags());

                Pop<Head>();
            }
            #endregion
        }
    }
}
