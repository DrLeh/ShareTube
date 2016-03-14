using ShareTube.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShareTube.Models.Tracking
{
    public interface ITrackingService
    {
        Task Track(ActionExecutingContext filterContext);
        IEnumerable<TrackingEntry> GetTrackings(int skip, int take);
    }
}