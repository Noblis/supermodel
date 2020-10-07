using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MvcCoreTester.Auth;
using MvcCoreTester.DataContexts;
using Supermodel.Presentation.Mvc.Bootstrap4.Startup;

namespace MvcCoreTester
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSupermodelMvcBs4Services<SecureAuthenticationHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSupermodelMvcBs4Middleware<DataContext>(env);
        }
    }
}
