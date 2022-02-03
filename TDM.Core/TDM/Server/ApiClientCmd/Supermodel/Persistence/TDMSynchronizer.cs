#nullable enable

using System;
using ApiClientCmd.Supermodel.Auth;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.DataContext.Offline;

namespace ApiClientCmd.Supermodel.Persistence
{
    public class TDMSynchronizer : Synchronizer<ToDoList, TDMWebApiDataContext, TDMSqliteDataContext>
    {
        public override DateTime GetModifiedDateTimeUtc(ToDoList model)
        {
            return model.ModifiedOnUtc;
        }

        public override DateTime GetCreatedDateTimeUtc(ToDoList model)
        {
            return model.CreatedOnUtc;
        }

        protected override void SetUpWebApiContext(TDMWebApiDataContext context)
        {
            context.CustomValues["InSynchronizer"] = true; 
            context.AuthHeader = new TDMSecureAuthHeaderGenerator("ilya.basin@noblis.org", "0", Array.Empty<byte>()).CreateAuthHeader();
        }

        protected override void SetUpSqliteContext(TDMSqliteDataContext context)
        {
            context.CustomValues["InSynchronizer"] = true; 
        }
    }
}
