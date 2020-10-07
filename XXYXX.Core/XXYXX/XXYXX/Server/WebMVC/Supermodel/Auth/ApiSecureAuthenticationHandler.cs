using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Supermodel.Encryptor;
using Supermodel.Persistence.EFCore;
using Supermodel.Persistence.Repository;
using Supermodel.Persistence.UnitOfWork;
using Supermodel.Presentation.Mvc.Auth;

namespace WebMVC.Supermodel.Auth
{
    public class ApiSecureAuthenticationHandler: SupermodelApiAuthenticationHandlerBase
    {
        #region Constructors
        public ApiSecureAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock){}
        #endregion

        #region Overrides
        protected override Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
        {
            throw new InvalidOperationException(); 
        }
        protected override async Task<List<Claim>> AuthenticateEncryptedAndGetClaimsAsync(string[] args)
        {
            if (args.Length != 4) return new List<Claim>();

            await using (new UnitOfWork<DataContext>(ReadOnly.Yes))
            {
                var utcNow = DateTime.UtcNow;

                var username = args[0];
                var password = args[1];
                var secretTokenHash = args[2];
                var secretTokenHashSalt = args[3];

                var repo = (EFCoreSimpleDataRepo<XXYXXUser>)RepoFactory.Create<XXYXXUser>();
                var lowerCaseUsername =  username.ToLower();
                var user = repo.Items.SingleOrDefault(u => u.Username.ToLower() == lowerCaseUsername);

                var secretTokenValid = false;

                if (user != null && !string.IsNullOrEmpty(user.Username))
                {
                    var dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow.AddMinutes(-5));
                    if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;

                    dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow);
                    if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;

                    dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow.AddMinutes(5));
                    if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;
                }

                if (user != null && user.PasswordEquals(password) && secretTokenValid)
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
        #endregion

        #region Properties
        protected override byte[] EncryptionKey => Key;
        protected override string AuthHeaderName => HeaderName;
        #endregion

        #region Shared Constants
        public static readonly byte[] Key = { 0xAA, 0x68, 0x12, 0xB1, 0x35, 0x22, 0x51, 0xA0, 0xB2, 0x41, 0x27, 0x5C, 0x23, 0x9C, 0xF0, 0xDD };
        public static readonly string HeaderName = "X-XXYXX-Authorization";
        // ReSharper disable StringLiteralTypo
        public static readonly string SecretToken = "JHFV_jhaegvdkjHGVBKJDHgbejdfh**&@$vJHgvkzsdhfbgkb37r3t84r7glSCGO834FG{YD^%^$";
        // ReSharper restore StringLiteralTypo
        #endregion
    }
}
