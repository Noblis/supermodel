using System.Threading.Tasks;
using Supermodel.Mobile.Runtime.Common.DataContext.Sqlite;

namespace XFormsTester.Supermodel.Persistence
{
    public class TestSQliteDataContext: SqliteDataContext
    {
        #region Overrides
        //Optionally: Put your DbFileName here. For exmample if you need use multiple dbs 
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
