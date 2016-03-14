using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareTube.Data;

namespace ShareTube.Models
{
    public class RoomListViewModel
    {
        public IEnumerable<RoomListItem> Rooms { get; set; }
        public bool ShowCreate { get; set; }
    }
}