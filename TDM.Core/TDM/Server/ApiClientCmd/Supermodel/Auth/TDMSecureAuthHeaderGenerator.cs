﻿using System;
using Supermodel.Encryptor;
using Supermodel.Mobile.Runtime.Common.XForms.Pages.Login;

namespace ApiClientCmd.Supermodel.Auth
{
    public class TDMSecureAuthHeaderGenerator : BasicAuthHeaderGenerator
    {
        #region Shared Constants
        public static readonly byte[] Key = { 0xAA, 0x68, 0x12, 0xB1, 0x35, 0x22, 0x51, 0xA0, 0xB2, 0x41, 0x27, 0x5C, 0x23, 0x9C, 0xF0, 0xDD };
        public static readonly string HeaderName = "X-TDM-Authorization";
        // ReSharper disable StringLiteralTypo
        public static readonly string SecretToken = "[SECRET_TOKEN]";
        // ReSharper restore StringLiteralTypo
        #endregion

        #region Constructors
        public TDMSecureAuthHeaderGenerator(string username, string password, byte[] localStorageEncryptionKey) : base(username, password, localStorageEncryptionKey){}
        #endregion

        #region Overrdies
        public override AuthHeader CreateAuthHeader()
        {
            var dateTimeSalt = HashAgent.Generate5MinTimeStampSalt(DateTime.UtcNow);
            var secretTokenHashSalt = HashAgent.GenerateGuidSalt();
            var secretTokenHash = HashAgent.HashPasswordSHA256(SecretToken + dateTimeSalt, secretTokenHashSalt);
            var authHeader = HttpAuthAgent.CreateSMCustomEncryptedAuthHeader(Key, Username, Password, secretTokenHash, secretTokenHashSalt);
            authHeader.HeaderName = HeaderName;
            return authHeader;
        }
        #endregion
    }
}
