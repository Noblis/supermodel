#nullable enable

using System;
using System.Threading.Tasks;

namespace WebMonk.Filters.Base
{
    public readonly struct ActionFilterResult
    {
        #region Constructors
        public ActionFilterResult(bool abortProcessing, bool abortFurtherRouting, Func<Task>? executeResultFuncAsync)
        {
            if (!abortProcessing && abortFurtherRouting) throw new ArgumentException("abortFurtherRouting && !abortProcessing is not a valid combination");
            
            AbortProcessing = abortProcessing;
            AbortFurtherRouting = abortFurtherRouting;
            ExecuteResultFuncAsync = executeResultFuncAsync;
        }
        #endregion

        #region Properties
        public bool AbortProcessing { get; }
        public bool AbortFurtherRouting { get; }
        public Func<Task>? ExecuteResultFuncAsync { get; }

        //public static ActionFilterResult Done { get; } = new ActionFilterResult(true, true);
        public static ActionFilterResult Proceed { get; } = new ActionFilterResult(false, false, null);
        public static ActionFilterResult Skip { get; } = new ActionFilterResult(true, false, null);
        #endregion
    }
}