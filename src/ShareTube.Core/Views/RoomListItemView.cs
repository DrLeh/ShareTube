using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Views
{
    public class RoomListItemView
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = null!;
        public int UserCount { get; set; }
    }
}
