#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Auth;

namespace WebWM.Supermodel.Auth
{
    public class ApiBasicAuthenticateAttribute : SupermodelAuthenticateAttributeBase
    {
        protected override async Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
        {
            await using(new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var repo = LinqRepoFactory.Create<XXYXXUser>();
                var lowerCaseUsername =  username.ToLower();
                var user = repo.Items.SingleOrDefault(u => u.Username.ToLower() == lowerCaseUsername);
                
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

        protected override Task<List<Claim>> AuthenticateEncryptedAndGetClaimsAsync(string[] args)
        {
            throw new InvalidOperationException(); 
        }

        #region Properties
        protected override byte[] EncryptionKey => throw new InvalidOperationException(); 
        #endregion
    }
}
