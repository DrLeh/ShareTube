using ShareTube.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Services
{
    public interface IRoomService
    {
    }

    public class RoomService : IRoomService
    {
        private readonly IRoomLoader _roomLoader;

        public RoomService(IRoomLoader roomLoader)
        {
            _roomLoader = roomLoader;
        }
    }
}
