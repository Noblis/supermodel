#nullable enable

using Supermodel.Presentation.WebMonk.Context;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;
using WMWeb.Mvc.AuthPage;
using WMWeb.Mvc.HomePage;
using WMWeb.Mvc.InlineToDoListPage;
using WMWeb.Mvc.TDMUserUpdatePasswordPage;
using WMWeb.Mvc.ToDoListPage;

namespace WMWeb.Mvc.Layouts
{
    public class MasterMvcLayout : EmptyMvcLayout
    {
        public override IGenerateHtml RenderDefaultLayout()
        {
            return base.RenderDefaultLayout().FillBodySectionWith(new Tags
            { 
                new Nav(new { @class="navbar navbar-expand-sm navbar-dark bg-primary"})
                {
                    new Button(new { @class="navbar-toggler", type="button", data_toggle="collapse", data_target="#navbarSupportedContent", aria_controls="navbarSupportedContent", aria_expanded="false", aria_label="Toggle navigation" })
                    {
                        new Span(new { @class="navbar-toggler-icon" })
                    },

                    new Div(new { @class="collapse navbar-collapse", id="navbarSupportedContent" })
                    {
                        new Ul(new { @class="navbar-nav mr-auto" })
                        {
                            new Li(new { @class="nav-item" })
                            {
                                Render.ActionLink<HomeMvcController>("Home", x => x.GetIndex(), new { @class="nav-link active" } )
                            },
                            new Li(new { @class="nav-item dropdown" })
                            {
                                new A(new { @class="nav-link dropdown-toggle active", href="#", id="navbarDropdown1", role="button", data_toggle="dropdown", aria_haspopup="true", aria_expanded="false" })
                                {
                                    new Txt("Menu Group")
                                },
                                new Div(new { @class="dropdown-menu", aria_labelledby="navbarDropdown" })
                                {
                                    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                                    Render.ActionLink<ToDoListMvcController>("My To Do Lists", x => x.GetListAsync(null, 25, null), new { @class="dropdown-item" }),
                                    Render.ActionLink<InlineToDoListMvcController>("My To Do Lists Inline", x => x.GetListAsync(null, 25, null), new { @class="dropdown-item" }),
                                    #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                                }
                            }
                        },
                        new Ul(new { @class="nav-item navbar-nav navbar-right" })
                        {
                            new Li(new { @class="nav-item dropdown" })
                            {
                                new A(new { @class="nav-link dropdown-toggle active", href="#", id="navbarDropdown2", role="button", data_toggle="dropdown", aria_haspopup="true", aria_expanded="false"})
                                {
                                    new Txt($"Welcome, {RequestHttpContext.CurrentUserLabel}")
                                },
                                new Div(new { @class="dropdown-menu", aria_labelledby="navbarDropdown" })
                                {
                                    Render.ActionLink<TDMUserUpdatePasswordMvcController>("Change Password", x => x.GetDetailAsync(RequestHttpContext.CurrentUserId!.Value), new { @class="dropdown-item" }),
                                    Render.ActionLink<AuthMvcController>("Sign Out", x => x.GetLogOut(), new { @class="dropdown-item" }),
                                }
                            }
                        }
                    }
                },
                new Br(),
                new Div(new { id="body" }) 
                {
                    new BodySectionPlaceholder()
                }
            });
        }
    }
}
