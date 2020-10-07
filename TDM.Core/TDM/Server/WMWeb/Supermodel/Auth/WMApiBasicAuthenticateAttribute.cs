﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.WebMonk.Auth;
using WMDomain.Entities;
using WMDomain.Supermodel.Persistence;

namespace WMWeb.Supermodel.Auth
{
    public class WMApiBasicAuthenticateAttribute : SupermodelAuthenticateAttributeBase
    {
        protected override async Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
        {
            await using(new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var repo = LinqRepoFactory.Create<TDMUser>();
                var lowerCaseUsername =  username.ToLower();
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

        protected override Task<List<Claim>> AuthenticateEncryptedAndGetClaimsAsync(string[] args)
        {
            throw new InvalidOperationException(); 
        }

        #region Properties
        protected override byte[] EncryptionKey => throw new InvalidOperationException(); 
        #endregion
    }
}
