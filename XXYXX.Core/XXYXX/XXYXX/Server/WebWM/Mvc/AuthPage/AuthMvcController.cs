#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Auth;
using Supermodel.Presentation.WebMonk.Bootstrap4.Models;
using Supermodel.Presentation.WebMonk.Controllers.Mvc;
using WebMonk.Results;
using WebWM.Mvc.HomePage;

namespace WebWM.Mvc.AuthPage
{
    public class AuthMvcController : SimpleAuthMvcController<Bs4.LoginMvcModel, AuthMvcView>
    {
        #region Overrides
        protected override async Task<List<Claim>> AuthenticateAndGetClaimsAsync(string username, string password)
        {
            await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var repo = LinqRepoFactory.Create<XXYXXUser>();
                var lowerCaseUsername = username.ToLower();
                var user = repo.Items.SingleOrDefault(u => u.Username.ToLower() == lowerCaseUsername);

                if (user != null && user.PasswordEquals(password))
                {
                    var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(user.Id, user.Username);
                    return claims;
                }
                else
                {
                    //If user does not authenticate return empty list of claims
                    return new List<Claim>();
                }
            }
        }

        protected override LocalRedirectResult RedirectToHomeScreen()
        {
            return RedirectToAction<HomeMvcController>(x => x.GetIndex());
        }
        #endregion
    }
}