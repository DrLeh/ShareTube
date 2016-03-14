using ShareTube.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShareTube.Models.Tracking
{
    public class TrackingService : ITrackingService
    {
        public async Task Track(ActionExecutingContext filterContext)
        {
            await Task.Run(() => TrackAsync(filterContext));
        }

        private void TrackAsync(ActionExecutingContext filterContext)
        {
            using (var ctx = new ShareTubeDataContext())
            {
                var httpContext = filterContext.RequestContext.HttpContext;
                var routeData = httpContext.Request.RequestContext.RouteData;
                var action = routeData.GetRequiredString("action");
                var controller = routeData.GetRequiredString("controller");
                var entry = new TrackingEntry
                {
                    RequestedUrl = httpContext.Request.Url.AbsoluteUri,
                    UserAgentString = httpContext.Request.UserAgent,
                    Controller = controller,
                    Action = action,
                    IPAddress = httpContext.Request.ServerVariables["REMOTE_ADDR"]
                };
                ctx.TrackingEntries.Add(entry);
                ctx.SaveChanges();
            }
        }

        public IEnumerable<TrackingEntry> GetTrackings(int skip, int take)
        {
            using (var ctx = new ShareTubeDataContext())
            {
                return ctx.TrackingEntries.OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take).ToList();
            }
        }
    }
}