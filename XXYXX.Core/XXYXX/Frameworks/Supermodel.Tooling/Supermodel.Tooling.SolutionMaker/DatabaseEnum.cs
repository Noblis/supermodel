using System.ComponentModel;

#nullable enable

namespace Supermodel.Tooling.SolutionMaker
{
    public enum DatabaseEnum 
    { 
        Sqlite, 
        [Description("Sql Server")] SqlServer 
    }
}