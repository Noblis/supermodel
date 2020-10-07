#nullable enable

using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Supermodel.Persistence.Repository;

namespace Supermodel.Persistence.EFCore.SQLite
{
    public abstract class EFCoreSQLiteDataContext : EFCoreDataContext
    {
        #region Constructors
        protected EFCoreSQLiteDataContext(string connectionString, IRepoFactory? customRepoFactory = null)
            : base(connectionString, customRepoFactory) 
        { 
            // ReSharper disable once StringLiteralTypo
            //if in-memory db, we need to retain the connection
            if (connectionString.Trim().ToLower() != "datasource=:memory:") 
            { 
                Connection = new SqliteConnection(connectionString);
            }
            else
            {
                if (connectionString == MemoryConnectionStringRetainer && MemoryConnectionRetainer != null)
                {
                    Connection = MemoryConnectionRetainer;
                }
                else
                {
                    MemoryConnectionStringRetainer = connectionString;                    
                    MemoryConnectionRetainer = Connection = new SqliteConnection(connectionString);
                    Connection.Open();
                }
            }
        }
        #endregion

        #region Overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(Connection)
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging();
        }
        #endregion

        #region Methods
        public static string DbFilePath
        {
            get 
            { 
                //this is to make Cmd project point to the same db as web projects
                var workingPath = Directory.GetCurrentDirectory();
                if (workingPath.Contains("\\bin\\Debug\\")) return "../../../../"; 
                return "../"; 
            }
        }
        #endregion

        #region Properties
        public SqliteConnection Connection { get; protected set; }

        //we need this for in memory db. In order for in memory db not to be disposed, connection must not be disposed
        public static string? MemoryConnectionStringRetainer { get; protected set; }
        public static SqliteConnection? MemoryConnectionRetainer { get; protected set; }
        #endregion
    }
}

