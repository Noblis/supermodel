using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Supermodel.Encryptor;
using Supermodel.Presentation.Mvc.Auth;

namespace MvcCoreTester.Auth
{
    public class SecureAuthenticationHandler : SupermodelApiAuthenticationHandlerBase
    {
        #region Shared Constants
        public static readonly byte[] Key = { 0xAA, 0x68, 0x12, 0xB1, 0x35, 0x22, 0x51, 0xA0, 0xB2, 0x41, 0x27, 0x5C, 0x23, 0x9C, 0xF0, 0xDD };
        public static readonly string HeaderName = "X-TDM-Authorization";
        public static readonly string SecretToken = "[SECRET_TOKEN]";
        #endregion

        public SecureAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock){}

        protected override Task<List<Claim>> AuthenticateBasicAndGetClaimsAsync(string username, string password)
        {
            //if (username.ToLower() == "ilya.basin@gmail.org" && password == "0")
            //{
            //    var claims = AuthClaimsHelper.CreateNewClaimsListWithIdAndLabel(1, "Ilya Basin");
            //    claims.Add(new Claim(ClaimTypes.Role, "Adder", ClaimValueTypes.String));
            //    return Task.FromResult(claims);
            //}
            //else
            //{
            //    return Task.FromResult(new List<Claim>());
            //}
            throw new InvalidOperationException(); 
        }

        protected override Task<List<Claim>> AuthenticateEncryptedAndGetClaimsAsync(string[] args)
        {
            if (args.Length != 4) return Task.FromResult(new List<Claim>());

            var utcNow = DateTime.UtcNow;

            var username = args[0];
            var password = args[1];
            var secretTokenHash = args[2];
            var secretTokenHashSalt = args[3];

            var secretTokenValid = false;

            var dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow.AddMinutes(-5));
            if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;

            dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow);
            if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;

            dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(utcNow.AddMinutes(5));
            if (HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt) == secretTokenHash) secretTokenValid = true;

            if (username.ToLower() == "ilya.basin@gmail.org" && password == "0" && secretTokenValid)
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

        #region Properties
        protected override byte[] EncryptionKey => Key;
        protected override string AuthHeaderName => HeaderName;
        #endregion
    }
}
