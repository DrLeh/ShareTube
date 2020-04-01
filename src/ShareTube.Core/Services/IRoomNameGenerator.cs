using ShareTube.Core.Data;
using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareTube.Core.Services
{
    public interface IRoomNameGenerator
    {
        string GenerateRoomName();
    }

    public class RoomNameGenerator : IRoomNameGenerator
    {
        private readonly IDataAccess _dataAccess;

        public RoomNameGenerator(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public string GenerateRoomName()
        {
            string format = "ShareTube Room - {0}";
            string name = "";
            var random = new Random();
            var roomCount = _dataAccess.Query<Room>().Count();

            //random size scales with total room count, to make it more unlikely that 
            // you find a match when finding a unique room name
            //should just be number of rooms + 1 digit, aka * 10? Rounded up one digit?
            var countMod10 = ((int)(roomCount / 10));
            var randomSize = 100 * (countMod10 == 0 ? 1 : countMod10);
            if (randomSize > 10000)
                randomSize = 10000;
            randomSize--;

            while (true)
            {
                name = string.Format(format, random.Next(randomSize));
                if (!_dataAccess.Query<Room>().Any(x => x.Name == name))
                    break;
            }
            return name;
        }
    }
}
