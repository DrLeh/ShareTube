using ShareTube.App_Start;
using ShareTube.Models.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace ShareTube.Filter
{
    public class TrackingAttribute   : FilterAttribute, IActionFilter
    {
        public bool Track { get; set; } = true;
        public TrackingAttribute(bool track)
        {
            Track = track;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Track)
            {
                var trackingService = UnityConfig.GetConfiguredContainer().Resolve<ITrackingService>();
                trackingService.Track(filterContext);
            }
        }
    }
}