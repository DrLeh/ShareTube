using ShareTube.Core.Data;
using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareTube.Core.Loaders
{
    public interface IRoomLoader
    {
        List<Room> GetPublicRooms();
    }

    public class RoomLoader : IRoomLoader
    {
        private readonly IDataAccess _dataAccess;

        public RoomLoader(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<Room> GetPublicRooms()
        {
            return _dataAccess.Query<Room>()
                .Where(x => !x.IsPrivate)
                .ToList();
        }
    }
}
