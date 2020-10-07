#nullable enable

using Supermodel.Presentation.WebMonk.Bootstrap4.TagComponents;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;

namespace WebWM.Mvc.Layouts
{
    public class EmptyModalMvcLayout : EmptyMvcLayout
    {
        public override IGenerateHtml RenderDefaultLayout()
        {
            return base.RenderDefaultLayout().FillBodySectionWith(new Tags
            {
                new ModalContainer("dialog", backgroundColor: "SkyBlue")
                {
                    new BodySectionPlaceholder()
                },
                new Script 
                { 
                    new Txt
                    (@"
                        $(function() {
                            $('#dialog').modal({backdrop: 'static', keyboard: false});
                        });
                    ")
                },
            });
        }
    }
}
