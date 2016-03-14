using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using ShareTube.Data;
using ShareTube.Hubs;
using ShareTube.Infrastructure;

[assembly: OwinStartup(typeof(ShareTube.Startup))]
namespace ShareTube
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //GlobalHost.DependencyResolver.Register(
            //    typeof(ShareTubeHub),
            //    () => new ShareTubeHub(new ShareTubeClientContract()));
            GlobalHost.DependencyResolver.Register(
                            typeof(ShareTubeHub),
                            () => new ShareTubeHub(new ShareTubeService(), new ShareTubeClientContract(), new CookieHelper()));

            app.MapSignalR();
        }
    }
}
