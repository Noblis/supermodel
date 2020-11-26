using WebMonk.ValueProviders;

#nullable enable

namespace WebMonk.Context
{
    public interface IRouteManager
    {
        string BaseUrl { get; }
        string LocalPath { get; }
        string QueryString { get; }
        string[] LocalPathParts { get; }
        string OverridenHttpMethod { get; set; }
        string LocalPathWithQueryString { get; }
        string LocalPathWithQueryStringMinusSelectedId { get; }

        string GetController();
        string? GetAction();

        RouteValueProvider RouteValueProvider { get; }
    }
}