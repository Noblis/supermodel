using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Controllers.Mvc;

namespace MvcCoreTester.Controllers
{
    public class AuthController : SimpleAuthController<Bs4.LoginMvcModel>
    {
        protected override Task<List<Claim>> AuthenticateAndGetClaimsAsync(string username, string password)
        {
            if (username.ToLower() == "ilya.basin@gmail.com" && password == "0")
            {
                var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(1, "Ilya Basin");
                claims.Add(new Claim(ClaimTypes.Role, "Adder", ClaimValueTypes.String));
                return Task.FromResult(claims);
            }
            else
            {
                return Task.FromResult(new List<Claim>());
            }
        }

        protected override IActionResult RedirectToHomeScreen()
        {
            return RedirectToAction("List", "Student",new { smTake = 5});
        }
    }
}
