#nullable enable

using System;
using System.Text;
using System.Threading.Tasks;
using Supermodel.ApiClient.Models;
using Supermodel.Mobile.Runtime.Common.DataContext.Sqlite;
using Supermodel.Mobile.Runtime.Common.Repository;
using Supermodel.Mobile.Runtime.Common.UnitOfWork;
using Supermodel.ReflectionMapper;

namespace ApiClientCmd.Supermodel.Persistence
{
    public class TDMSqliteDataContext : SqliteDataContext
    {
        #region Overrides
        //Optionally: Put your DbFileName here. For exmample if you need use multiple dbs 
        // ReSharper disable once RedundantOverriddenMember
        public override string DbFileName => base.DbFileName;

        //Optionally: Put your schema version here 
        // ReSharper disable once RedundantOverriddenMember
        public override int ContextSchemaVersion => 2;

        //Optionally: Put your schema migration code here
        // ReSharper disable once RedundantOverriddenMember
        public override async Task MigrateDbAsync(int? fromVersion, int toVersion)
        {
            if (fromVersion == 1 && toVersion == 2)
            {
                await using (new UnitOfWork<TDMSqliteDataContext>())
                {
                    var allToDoLists = await RepoFactory.Create<ToDoList>().GetAllAsync();
                    foreach (var toDoList in allToDoLists)
                    {
                        //Forces to update even if no changes were detected
                        toDoList.Update();
                    }
                }
            }
            else
            {
                await base.MigrateDbAsync(fromVersion, toVersion);
            }
        }

        public override string GetWhereClause<TModel>(object searchBy, string sortBy)
        {
            var result = new StringBuilder();

            var searchTerm = (string?)searchBy.PropertyGet(nameof(SimpleSearch.SearchTerm));
            if (!string.IsNullOrEmpty(searchTerm)) result.Append($"AND Index0 LIKE '%{searchTerm}%'");

            if (!string.IsNullOrEmpty(sortBy)) throw new Exception("Unexpected sortBy");

            return result.ToString();
        }

        public override string? GetStringIndex<TModel>(int idxNum0To9, TModel model)
        {
            if (model is ToDoList toDoList)
            {
                if (idxNum0To9 == 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(toDoList.Name);
                    foreach (var toDoItem in toDoList.ToDoItems) sb.AppendLine(toDoItem.Name);
                    return sb.ToString();
                }
                return null;
            }
            return base.GetStringIndex(idxNum0To9, model);
        }
        #endregion
    }
}
