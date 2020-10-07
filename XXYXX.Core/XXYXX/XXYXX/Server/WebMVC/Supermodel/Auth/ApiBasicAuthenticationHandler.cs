using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Mvc.Auth;

namespace WebMVC.Supermodel.Auth
{
    public class ApiBasicAuthenticationHandler : SupermodelApiAuthenticationHandlerBase
    {
        #region Constructors
        public ApiBasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock) { }
        #endregion

        #region Overrides
        protected override async Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
        {
            await using(new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var lowercaseUsername = username.ToLower();
                var user = await LinqRepoFactory.Create<XXYXXUser>().Items.SingleOrDefaultAsync(x => x.Username.ToLower() == lowercaseUsername);

                if (user != null && user.PasswordEquals(password))
                {
                    var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(user.Id, $"{user.Username}");

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
        protected override Task<List<Claim>> AuthenticateEncryptedAndGetClaimsAsync(string[] args)
        {
            throw new InvalidOperationException(); 
        }
        #endregion

        #region Properties
        protected override byte[] EncryptionKey => throw new InvalidOperationException(); 
        #endregion
    }
}
