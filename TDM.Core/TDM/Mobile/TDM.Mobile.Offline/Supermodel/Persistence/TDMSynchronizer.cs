#nullable enable

using System;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.DataContext.Offline;
using TDM.Mobile.AppCore;

namespace TDM.Mobile.Supermodel.Persistence
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
            context.AuthHeader = TDMApp.RunningApp.AuthHeaderGenerator.CreateAuthHeader();
        }

        protected override void SetUpSqliteContext(TDMSqliteDataContext context)
        {
            context.CustomValues["InSynchronizer"] = true; 
        }
    }
}
