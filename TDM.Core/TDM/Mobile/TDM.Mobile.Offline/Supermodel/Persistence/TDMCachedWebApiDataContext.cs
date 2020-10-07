#nullable enable

using Supermodel.Mobile.Runtime.Common.DataContext.CachedWebApi;

namespace TDM.Mobile.Supermodel.Persistence
{
    public class TDMCachedWebApiDataContext : CachedWebApiDataContext<TDMWebApiDataContext, TDMSqliteDataContext> {}
}
