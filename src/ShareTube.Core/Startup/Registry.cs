using Microsoft.Extensions.DependencyInjection;
using ShareTube.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
//using ShareTube.Core.Security;
using ShareTube.Core.Context;
using ShareTube.Core.Services;
using ShareTube.Core.Loaders;
using Microsoft.Extensions.Configuration;
using ShareTube.Core.Configuration;
using ShareTube.Core.Security;

namespace ShareTube.Core.Startup
{
    public class Registry
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<Configuration.IConfiguration, FileConfiguration>();

            services.AddScoped<ISecurityPolicy, PermissiveSecurityPolicy>();

            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomLoader, RoomLoader>();
            services.AddScoped<IRoomNameGenerator, RoomNameGenerator>();
        }
    }
}
