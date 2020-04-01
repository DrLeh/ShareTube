using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTube.Web.Startup
{
    public class Registry
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddSignalR();

            new Core.Startup.Registry().RegisterServices(services);
            new Data.Startup.Registry().RegisterServices(services);
        }
    }
}
