#nullable enable

using Domain.Supermodel.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Supermodel.Presentation.Mvc.Bootstrap4.Startup;
using WebMVC.Supermodel.Auth;

namespace WebMVC
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Pick one of these for web api authentication
            //services.AddSupermodelMvcBs4Services<ApiSecureAuthenticationHandler>();
            services.AddSupermodelMvcBs4Services<ApiBasicAuthenticationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSupermodelMvcBs4Middleware<DataContext>(env);
        }
    }
}
