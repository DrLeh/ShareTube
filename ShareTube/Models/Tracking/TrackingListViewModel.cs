using ShareTube.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube.Models.Tracking
{
    public class TrackingListViewModel
    {
        public IEnumerable<TrackingEntry> Trackings { get; set; }
        public int PageNumber { get; set; }
    }
}