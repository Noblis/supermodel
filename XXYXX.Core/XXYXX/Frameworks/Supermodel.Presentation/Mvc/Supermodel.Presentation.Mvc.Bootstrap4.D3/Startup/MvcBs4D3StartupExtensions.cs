#nullable enable

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Supermodel.DataAnnotations;
using Supermodel.Persistence.DataContext;
using Supermodel.Presentation.Mvc.Auth;
using Supermodel.Presentation.Mvc.Bootstrap4.Models;
using Supermodel.Presentation.Mvc.Bootstrap4.Startup;
using Supermodel.Presentation.Mvc.Bootstrap4.TagHelpers;
using Supermodel.Presentation.Mvc.Context;
using Supermodel.Presentation.Mvc.Startup;

namespace Supermodel.Presentation.Mvc.Bootstrap4.D3.Startup
{
    public static class MvcBs4D3StartupExtensions
    {
        #region Constructors
        static MvcBs4D3StartupExtensions()
        {
            D3Js = EmbeddedResource.ReadTextFile(typeof(MvcBs4D3StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.D3.StaticWebFiles.d3.v5.min.js");
            BrightChartsJs = EmbeddedResource.ReadTextFile(typeof(MvcBs4D3StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.D3.StaticWebFiles.britecharts.min.js");
            BrightChartsCss = EmbeddedResource.ReadTextFile(typeof(MvcBs4D3StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.D3.StaticWebFiles.britecharts.min.css");
        }
        #endregion

        #region Methods
        public static IMvcBuilder AddSupermodelMvcBs4D3Services(this IServiceCollection services, string? accessDeniedPath = null, string loginPath="/Auth/Login")
        {
            return services.AddSupermodelMvcBs4Services(accessDeniedPath, loginPath);
        }
        public static IMvcBuilder AddSupermodelMvcBs4D3Services<TApiAuthenticationHandler>(this IServiceCollection services, string? accessDeniedPath = null, string loginPath="/Auth/Login")
            where TApiAuthenticationHandler : SupermodelApiAuthenticationHandlerBase
        {
            return services.AddSupermodelMvcBs4Services<TApiAuthenticationHandler>(accessDeniedPath, loginPath);
        }
        public static IApplicationBuilder UseSupermodelMvcBs4D3Middleware<TDataContext>(this IApplicationBuilder builder, IWebHostEnvironment env, string? errorPage = null)
            where TDataContext : class, IDataContext, new()
        {
            SetUpMiddlewareWithoutEndpoints<TDataContext>(builder, env, errorPage);
            builder.UseEndpoints(SetUpEndpoints);  
            return builder;
        }

        public static void SetUpMiddlewareWithoutEndpoints<TDataContext>(IApplicationBuilder builder, IWebHostEnvironment env, string? errorPage)
            where TDataContext : class, IDataContext, new()
        {
            MvcBs4StartupExtensions.SetUpMiddlewareWithoutEndpoints<TDataContext>(builder, env, errorPage);
        }
        public static void SetUpEndpoints(IEndpointRouteBuilder endpoints)
        {
            MvcBs4StartupExtensions.SetUpEndpoints(endpoints);

            endpoints.MapGet("static_web_files/d3.v5.min.js", async context => { await context.Response.WriteAsync(D3Js); });
            endpoints.MapGet("static_web_files/britecharts.min.js", async context => { await context.Response.WriteAsync(BrightChartsJs); });
            endpoints.MapGet("static_web_files/britecharts.min.css", async context => { await context.Response.WriteAsync(BrightChartsCss); });
        }
        #endregion

        #region Properties
        public static string BrightChartsJs { get; }
        public static string BrightChartsCss { get; }
        public static string D3Js { get; }
        #endregion
    }
}
