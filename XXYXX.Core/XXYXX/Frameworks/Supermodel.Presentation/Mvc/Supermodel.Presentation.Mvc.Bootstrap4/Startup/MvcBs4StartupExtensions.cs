﻿#nullable enable

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
using Supermodel.Presentation.Mvc.Bootstrap4.TagHelpers;
using Supermodel.Presentation.Mvc.Context;
using Supermodel.Presentation.Mvc.Startup;

namespace Supermodel.Presentation.Mvc.Bootstrap4.Startup
{
    public static class MvcBs4StartupExtensions
    {
        #region Constructors
        static MvcBs4StartupExtensions()
        {
            JqueryJs = EmbeddedResource.ReadTextFile(typeof(MvcBs4StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.StaticWebFiles.super.bs4.js");
            SuperBs4Js = EmbeddedResource.ReadTextFile(typeof(MvcBs4StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.StaticWebFiles.super.bs4.js");
            SuperBs4Css = EmbeddedResource.ReadTextFile(typeof(MvcBs4StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.StaticWebFiles.super.bs4.css");
            BootboxJs = EmbeddedResource.ReadTextFile(typeof(MvcBs4StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.StaticWebFiles.bootbox.all.min.js");

            MessageHtml = EmbeddedResource.ReadTextFile(typeof(MvcBs4StartupExtensions).Assembly, "Supermodel.Presentation.Mvc.Bootstrap4.StaticWebFiles.Message.html");
        }
        #endregion

        #region Methods
        public static IMvcBuilder AddSupermodelMvcBs4Services(this IServiceCollection services, string? accessDeniedPath = null, string loginPath="/Auth/Login")
        {
            accessDeniedPath ??= Bs4.Message.RegisterMultiReadMessageAndGetUrl("403 Forbidden: Access Denied");
            return services.AddSupermodelMvcServices(accessDeniedPath, loginPath);
        }
        public static IMvcBuilder AddSupermodelMvcBs4Services<TApiAuthenticationHandler>(this IServiceCollection services, string? accessDeniedPath = null, string loginPath="/Auth/Login")
            where TApiAuthenticationHandler : SupermodelApiAuthenticationHandlerBase
        {
            accessDeniedPath ??= Bs4.Message.RegisterMultiReadMessageAndGetUrl("403 Forbidden: Access Denied");
            return services.AddSupermodelMvcServices<TApiAuthenticationHandler>(accessDeniedPath, loginPath);
        }
        public static IApplicationBuilder UseSupermodelMvcBs4Middleware<TDataContext>(this IApplicationBuilder builder, IWebHostEnvironment env, string? errorPage = null)
            where TDataContext : class, IDataContext, new()
        {
            SetUpMiddlewareWithoutEndpoints<TDataContext>(builder, env, errorPage);
            builder.UseEndpoints(SetUpEndpoints);  
            return builder;
        }

        public static void SetUpMiddlewareWithoutEndpoints<TDataContext>(IApplicationBuilder builder, IWebHostEnvironment env, string? errorPage)
            where TDataContext : class, IDataContext, new()
        {
            errorPage ??= Bs4.Message.RegisterMultiReadMessageAndGetUrl("500 Internal Server Error");

            if (env.IsDevelopment())
            {
                builder.UseDeveloperExceptionPage();
            }
            else
            {
                builder.UseExceptionHandler(errorPage);
                builder.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }

            builder.UseStaticFiles();

            builder.UseSupermodelMvcMiddleware<TDataContext>();

            //builder.UseHttpsRedirection();
            builder.UseCookiePolicy();
            builder.UseSession();
        }
        public static void SetUpEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute("DefaultMvc", "{controller=Home}/{action=Index}/{id:long?}");
            endpoints.MapRazorPages();

            endpoints.MapGet("static_web_files/jquery-3.6.0.min.js", async context => { await context.Response.WriteAsync(SuperBs4Js); });
            endpoints.MapGet("static_web_files/super.bs4.js", async context => { await context.Response.WriteAsync(SuperBs4Js); });
            endpoints.MapGet("static_web_files/super.bs4.css", async context => { await context.Response.WriteAsync(SuperBs4Css); });
            endpoints.MapGet("static_web_files/bootbox.all.min.js", async context => { await context.Response.WriteAsync(BootboxJs); });

            endpoints.MapGet("static_web_files/Message.html", async context =>
            {
                var urlHelper = RequestHttpContext.GetUrlHelperWithEmptyViewContext();

                var messageHtml = MessageHtml
                    .Replace("[%HeaderSnippet%]", SuperBs4HeadTagHelper.GetSupermodelSnippetStatic(urlHelper))
                    .Replace("[%BodySnippet%]", SuperBs4BodyTagHelper.GetSupermodelSnippetStatic(urlHelper))
                    .Replace("[%MessageSnippet%]", Bs4.Message.ReadMessageText(context.Request.Query["msgGuid"]))
                    .Replace("[%HomePageSnippet%]", urlHelper.Content("~/"));

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(messageHtml);
            });
        }
        #endregion

            #region Properties
        public static string SuperBs4Js { get; }
        public static string SuperBs4Css { get; }
        public static string BootboxJs { get; }
        public static string JqueryJs { get; }

        public static string MessageHtml { get; }
        #endregion
    }
}
