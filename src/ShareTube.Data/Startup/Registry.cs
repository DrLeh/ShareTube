using Microsoft.Extensions.DependencyInjection;
using ShareTube.Core.Data;
using ShareTube.Data.Context;

namespace ShareTube.Data.Startup
{
    public class Registry
    {
        public void RegisterServices(IServiceCollection services)
        {
            //services.AddDbContext<ShareTubeDbContext>((sp, options) => options.UseSqlServer(sp
            //    .GetService<Core.Configuration.IConfiguration>().ConnectionString));
            //services.AddDbContext<ShareTubeDbContext>((sp, options) => options.use)

            services.AddScoped<IDataAccess, DataAccess>();
        }
    }
}
