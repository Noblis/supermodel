#nullable enable

using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;

namespace WMWeb.Mvc.Layouts
{
    public class EmptyModalMvcLayout : EmptyMvcLayout
    {
        public override IGenerateHtml RenderDefaultLayout()
        {
            return base.RenderDefaultLayout().FillBodySectionWith(new Tags
            {
                new Div(new { id="dialog", @class="modal fade", tabindex="-1", role="dialog", aria_labelledby="exampleModalLabel", aria_hidden="true" })
                {
                    new Div(new { @class="modal-dialog", role="document" })
                    {
                        new Div(new { @class="modal-content" })
                        {
                            new Div(new { @class="modal-body" })
                            {
                                new BodySectionPlaceholder()
                            }
                        }
                    }
                },
                new Script 
                { 
                    new Txt(
                        @"$(function() {
                            $('#dialog').modal({backdrop: 'static', keyboard: false});
                        });")
                }
            });
        }
    }
}
