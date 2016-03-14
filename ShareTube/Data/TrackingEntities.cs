using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareTube.Data
{
    public class TrackingEntry : ShareTubeEntity
    {
        [Key]
        public int ID { get; set; }
        public string RequestedUrl { get; set; }
        public string UserAgentString { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IPAddress { get; set; }
    }
}