#nullable enable

using System.Threading.Tasks;
using WebMonk.Context.WMHttpListenerObjects;

namespace WebMonk.ValueProviders
{
    public class QueryStringValueProvider : ValueProvider
    {
        #region Methods
        public virtual Task<IValueProvider> InitAsync(IHttpListenerRequest request)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (request == null) return Task.FromResult((IValueProvider)this); //had an exception in production where request was somehow null
            return base.InitAsync(request.QueryString);
        }
        #endregion
    }
}