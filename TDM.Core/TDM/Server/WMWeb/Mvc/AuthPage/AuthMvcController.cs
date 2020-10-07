#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;
using WebMonk.Results;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;
using WMWeb.Mvc.HomePage;

namespace WMWeb.Mvc.AuthPage
{
    public class AuthMvcController : SimpleAuthMvcController<Bs4.LoginMvcModel, AuthMvcView>
    {
        #region Overrides
        protected override async Task<List<Claim>> AuthenticateAndGetClaimsAsync(string username, string password)
        {
            await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var repo = LinqRepoFactory.Create<TDMUser>();
                var lowerCaseUsername = username.ToLower();
                var user = repo.Items.SingleOrDefault(u => u.Username.ToLower() == lowerCaseUsername);

                if (user != null && user.PasswordEquals(password))
                {
                    var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(user.Id, $"{user.FirstName} {user.LastName}");
                    //claims.Add(new Claim(ClaimTypes.Role, "Adder", ClaimValueTypes.String));
                    return claims;

                }
                else
                {
                    return new List<Claim>();
                }
            }
        }

        protected override LocalRedirectResult RedirectToHomeScreen()
        {
            //return RedirectToAction("Home", "Index");
            return RedirectToAction<HomeMvcController>(x => x.GetIndex());         
        }
        #endregion
    }
}
