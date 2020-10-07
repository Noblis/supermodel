#nullable enable

using WebMonk.RazorSharp.HtmlTags;
using WebMonk.RazorSharp.HtmlTags.BaseTags;
using WebMonk.Rendering.Views;

namespace WebMonkTester.AuthPage
{
    public class AuthMvcView : MvcView
    {
        public IGenerateHtml RenderLogin(LoginMvcModel login)
        {
            var html = new Tags
            {
                new Form(new { method = "POST", enctype = "multipart/form-data" })
                {
                    new Div(new { @class="form-group"})
                    {
                        Render.LabelFor(login, x => x.Username),
                        Render.TextBoxFor(login, x => x.Username, new { @class="form-control" }),
                        Render.ValidationMessageFor(login, x => x.Username, new { @class="invalid-feedback d-block" })
                    },
                    new Div(new { @class="form-group"})
                    {
                        Render.LabelFor(login, x => x.Password),
                        Render.PasswordFor(login, x => x.Password, new { @class="form-control" }),
                        Render.ValidationMessageFor(login, x => x.Password, new { @class="invalid-feedback d-block" })
                    },
                    new Button(new { type="submit", @class="btn btn-primary" }){ new Txt("Log In") },
                }
            };
            return ApplyToDefaultLayout(html);
        }
    }
}
