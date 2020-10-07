#nullable enable

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Controllers.Mvc;
using Supermodel.Presentation.Mvc.Extensions;

namespace WebMVC.Controllers
{
    public class AuthController : SimpleAuthController<Bs4.LoginMvcModel>
    {
        #region Overrides
        protected override async Task<List<Claim>> AuthenticateAndGetClaimsAsync(string username, string password)
        {
            await using(new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var lowercaseUsername = username.ToLower();
                var user = await LinqRepoFactory.Create<XXYXXUser>().Items.SingleOrDefaultAsync(x => x.Username.ToLower() == lowercaseUsername);

                if (user != null && user.PasswordEquals(password))
                {
                    var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(user.Id, $"{user.FirstName} {user.LastName}");

                    //Add claims for the specific permissions following the example below
                    //if (user.Admin) claims.Add(new Claim(ClaimTypes.Role, XXYXXUser.AdminRole, ClaimValueTypes.String));
                    
                    return claims;
                }
                else
                {
                    return new List<Claim>();
                }
            }        
        }
        protected override IActionResult RedirectToHomeScreen()
        {
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).RemoveControllerSuffix());
        }
        #endregion
    }
}
