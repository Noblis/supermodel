using Supermodel.Encryptor;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.Login;
using System;

namespace WebServicesCmdClientTester
{
    public class SecureAuthHeaderGenerator : BasicAuthHeaderGenerator
    {
        #region Constants
        public static readonly byte[] Key = { 0xAA, 0x68, 0x12, 0xB1, 0x35, 0x22, 0x51, 0xA0, 0xB2, 0x41, 0x27, 0x5C, 0x23, 0x9C, 0xF0, 0xDD };
        public static readonly string HeaderName = "X-TDM-Authorization";
        public static readonly string SecretToken = "JHFV_jhaegvdkjHGVBKJDHgbejdfh**&@$vJHgvkzsdhfbgkb37r3t84r7glSCGO834FG{YD^%^$";
        #endregion

        #region Constructors
        public SecureAuthHeaderGenerator(string username, string password, byte[] localStorageEncryptionKey) : base(username, password, localStorageEncryptionKey){}
        #endregion

        #region Overrdies
        public override AuthHeader CreateAuthHeader()
        {
            var dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(DateTime.UtcNow);
            var secretTokanHashSalt = HashAgent.GenerateGuidSalt();
            var secretTokenHash = HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokanHashSalt);
            var authHeader = HttpAuthAgent.CreateSMCustomEncryptedAuthHeader(Key, Username, Password, secretTokenHash, secretTokanHashSalt);
            authHeader.HeaderName = HeaderName;
            return authHeader;
        }
        #endregion
    }
}
