using System;

namespace ShareTube.Core.ViewModels
{
    public class RoomListItem
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
    }
}