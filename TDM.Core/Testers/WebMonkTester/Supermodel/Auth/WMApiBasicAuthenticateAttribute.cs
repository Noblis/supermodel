#nullable enable

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Supermodel.Presentation.WebMonk.Auth;

namespace WebMonkTester.Supermodel.Auth
{
    public class WMApiBasicAuthenticateAttribute : SupermodelAuthenticateAttributeBase
    {
        protected override Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
        {
            //if (username == "ilya.basin@gmail.com" && password == "1234") 
            if (username == "Aladdin" && password == "OpenSesame")                
            {
                var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(1, "Ilya Basin");
                //claims.Add(new Claim(ClaimTypes.Role, "Adder", ClaimValueTypes.String));
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
