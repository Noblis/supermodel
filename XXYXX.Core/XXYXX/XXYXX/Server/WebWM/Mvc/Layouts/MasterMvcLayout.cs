#nullable enable

using Supermodel.Presentation.WebMonk.Context;
using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;
using WebWM.Mvc.AuthPage;
using WebWM.Mvc.XXYXXUserUpdatePasswordPage;

namespace WebWM.Mvc.Layouts
{
    public class MasterMvcLayout : EmptyMvcLayout
    {
        public override IGenerateHtml RenderDefaultLayout()
        {
            return base.RenderDefaultLayout().FillBodySectionWith(new Tags
            { 
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
                            new Li(new { @class="nav-item dropdown", })
                            {
                                new A(new { @class="nav-link dropdown-toggle active", href="#", role="button", data_toggle="dropdown", aria_haspopup="true", aria_expanded="false", })
                                {
                                    new Txt("Menu"),
                                },
                                new Div(new { @class="dropdown-menu", aria_labelledby="navbarDropdown", })
                                {
                                    new A(new { href="#", @class="dropdown-item", })
                                    {
                                        new Txt("Dropdown Item 1"),
                                    },
                                    new A(new { href="#", @class="dropdown-item", })
                                    {
                                        new Txt("Dropdown Item 2"),
                                    },
                                    new A(new { href="#", @class="dropdown-item", })
                                    {
                                        new Txt("Dropdown Item 3"),
                                    },
                                },
                            },
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
                                    Render.ActionLink<XXYXXUserUpdatePasswordMvcController>("Change Password", x => x.GetDetailAsync(RequestHttpContext.CurrentUserId!.Value), new { @class="dropdown-item" }),
                                    Render.ActionLink<AuthMvcController>("Sign Out", x => x.GetLogOut(), new { @class="dropdown-item" })
                                }
                            }
                        }
                    }
                },
                new Br(),
                new Div(new { id="body" }) 
                {
                    new BodySectionPlaceholder()
                },
            });
        }
    }
}
