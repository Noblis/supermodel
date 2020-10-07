using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Supermodel.Presentation.Mvc.Auth;

namespace MvcCoreTester.Auth
{
    public class BasicAuthenticationHandler : SupermodelApiAuthenticationHandlerBase
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock){}

        protected override Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
        {
            if (username.ToLower() == "ilya.basin@gmail.org" && password == "0")
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

        protected override Task<List<Claim>> AuthenticateEncryptedAndGetClaimsAsync(string[] args)
        {
            throw new InvalidOperationException(); 
        }

        #region Properties
        protected override byte[] EncryptionKey => throw new InvalidOperationException(); 
        #endregion
    }
}
