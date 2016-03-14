using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace ShareTube
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Watch from Youtube",
                url: "watch",
                defaults: new { controller = MVC.ShareTube.Name, action = MVC.ShareTube.ActionNames.WatchYT, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = MVC.ShareTube.Name, action = MVC.ShareTube.ActionNames.Index, id = UrlParameter.Optional }
            );
        }
    }
}
