#nullable enable

using System.Security.Claims;
using WebMonk.Context;
using WebMonk.Extensions;
using WebMonk.HttpRequestHandlers.Controllers;
using WebMonk.Results;

namespace WebMonkTester.AuthPage
{
    public class AuthMvcController : MvcController
    {
        public ActionResult GetLogin()
        {
            var login = new LoginMvcModel();
            return new AuthMvcView().RenderLogin(login).ToHtmlResult();
        }

        public ActionResult PostLogin(LoginMvcModel login, string returnUrl)
        {
            if (!HttpContext.Current.ValidationResultList.IsValid) return new AuthMvcView().RenderLogin(login).ToHtmlResult();
            
            if (login.Username == "ilya.basin@gmail.com" && login.Password == "1234")
            {
                HttpContext.Current.AuthenticateSessionWithClaims(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, 1.ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimTypes.Role, "Admin"),
                });
                return RedirectToLocal(returnUrl);
            }
            else
            {
                return RedirectToAction<AuthMvcController>(x => x.GetLogin());
            }
        }
    }
}
