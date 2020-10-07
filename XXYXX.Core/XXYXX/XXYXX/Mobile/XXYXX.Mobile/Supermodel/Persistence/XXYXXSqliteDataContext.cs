#nullable enable

using System.Threading.Tasks;
using Supermodel.Mobile.Runtime.Common.DataContext.Sqlite;

namespace XXYXX.Mobile.Supermodel.Persistence
{
    public class XXYXXSqliteDataContext : SqliteDataContext
    {
        #region Overrides
        //Optionally: Put your DbFileName here. For example if you need use multiple dbs 
        // ReSharper disable once RedundantOverriddenMember
        public override string DbFileName => base.DbFileName;

        //Optionally: Put your schema version here 
        // ReSharper disable once RedundantOverriddenMember
        public override int ContextSchemaVersion => base.ContextSchemaVersion;

        //Optionally: Put your schema migration code here
        // ReSharper disable once RedundantOverriddenMember
        public override Task MigrateDbAsync(int? fromVersion, int toVersion)
        {
            return base.MigrateDbAsync(fromVersion, toVersion);
        }
        #endregion
    }
}
