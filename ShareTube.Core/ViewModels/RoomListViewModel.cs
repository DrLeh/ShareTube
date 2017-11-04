using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.ViewModels
{
    public class RoomListViewModel
    {
        public IEnumerable<RoomSearchResult> Rooms { get; set; }
    }
}
