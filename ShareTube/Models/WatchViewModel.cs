using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareTube.Models
{
    public class WatchViewModel
    {
        public Guid RoomID { get; set; }
        [Display(Name = "Room Name")]
        public string RoomName { get; set; }
    }
}