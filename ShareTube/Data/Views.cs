using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube.Data
{
    public class RoomListItem
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
    }
}