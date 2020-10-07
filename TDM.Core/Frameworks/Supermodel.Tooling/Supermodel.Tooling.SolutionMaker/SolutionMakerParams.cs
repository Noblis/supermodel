﻿#nullable enable

namespace Supermodel.Tooling.SolutionMaker
{
    public interface ISolutionMakerParams
    {
        #region Properties
        string SolutionName { get; } 
        string SolutionDirectory { get; }

        WebFrameworkEnum WebFramework { get; }
        MobileApiEnum MobileApi { get; }

        DatabaseEnum Database { get; }
        #endregion
    }
}
