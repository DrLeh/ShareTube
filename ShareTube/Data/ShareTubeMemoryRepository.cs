using ShareTube.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube.Data
{
    public class MemoryStorage
    {
        private List<PlayerStatus> _PlayerStatuses;
        public List<PlayerStatus> PlayerStatuses
        {
            get
            {
                if (_PlayerStatuses == null)
                {
                    _PlayerStatuses = new List<PlayerStatus>();
                    foreach (ShareTubePlayerStatus opt in Enum.GetValues(typeof(ShareTubePlayerStatus)))
                    {
                        _PlayerStatuses.Add(new PlayerStatus(opt));
                    }
                }
                return _PlayerStatuses;
            }
        }

        public List<Room> Rooms { get { return GetCollection<Room>(Constants.MemoryKeys.Room); } }
        public List<UserConnection> UserConnections { get { return GetCollection<UserConnection>(Constants.MemoryKeys.UserConnection); } }
        public List<Video> Videos { get { return GetCollection<Video>(Constants.MemoryKeys.Video); } }
        public List<User> Users { get { return GetCollection<User>(Constants.MemoryKeys.User); } }

        public List<T> GetCollection<T>(string key)
        {
            var ctx = HttpContext.Current;
            if (ctx == null)
                return null;
            var coll = (List<T>)ctx.Application[key];
            if (coll == null)
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[key] = coll = new List<T>();
                HttpContext.Current.Application.UnLock();
            }
            return coll;
        }
    }

    public class ShareTubeMemoryRepository : IShareTubeService
    {
        public static MemoryStorage ctx = new MemoryStorage();

        public List<RoomListItem> GetPublicRooms()
        {
            return ctx.Rooms.Where(x => x.IsPrivate == false)
                .Select(x => new RoomListItem
                {
                    ID = x.ID,
                    Name = x.Name,
                    UserCount = ctx.UserConnections.Count(uc => uc.RoomID == x.ID)
                }).ToList();
        }

        public Room AddRoom(string name = null)
        {
            if (name == null)
                name = GetUniqueRoomName();
            var room = new Room
            {
                Name = name
            };
            ctx.Rooms.Add(room);

            return room;
        }

        public void RemoveRoom(Guid roomID)
        {
            ctx.Rooms.Remove(ctx.Rooms.Single(x => x.ID == roomID));
        }

        public Room GetRoom(Guid roomID)
        {
            return GetRoom_Internal(roomID);
        }

        private Room GetRoom_Internal(Guid roomID)
        {
            return ctx.Rooms.Single(x => x.ID == roomID);
        }

        public Room GetOrAddRoom(Guid roomID)
        {
            var room = GetRoom_Internal(roomID);
            if (room == null)
            {
                room = new Room()
                {
                    Name = GetUniqueRoomName(),
                };
            }
            return room;
        }


        public string GetUniqueRoomName()
        {
            string format = "ShareTube Room - {0}";
            string name = "";
            var random = new Random();
            var roomCount = ctx.Rooms.Count();

            //random size scales with total room count, to make it more unlikely that 
            // you find a match when finding a unique room name
            var countMod10 = ((int)((float)roomCount / 10));
            var randomSize = 100 * (countMod10 == 0 ? 1 : countMod10);
            if (randomSize > 10000)
                randomSize = 10000;
            randomSize--;

            while (true)
            {
                name = string.Format(format, random.Next(randomSize));
                if (!ctx.Rooms.Any(x => x.Name == name))
                    break;
            }
            return name;
        }

        public void UpdateTime(Guid roomID, double time)
        {
            var room = GetRoom_Internal(roomID);
            room.CurrentTime = time;
        }

        public void UpdateStatus(Guid roomID, ShareTubePlayerStatus status)
        {
            var room = GetRoom_Internal(roomID);
            room.ShareTubePlayerStatus = status;
        }



        #region Users

        public void AddUser(Guid roomID, Guid userID, Guid connID, string username)
        {
            var user = ctx.Users.SingleOrDefault(x => x.ID == userID);
            if (user == null)
            {
                user = new User();
                user.Name = username;
                user.ID = userID;

                ctx.Users.Add(user);
            }
            if (user.Connections.Find(x => x.ID == connID) == null)
            {
                var conn = new UserConnection
                {
                    ID = connID,
                    UserID = user.ID,
                    RoomID = roomID
                };
                if (!ctx.UserConnections.Any(x => x.RoomID == roomID))
                    conn.IsHost = true;
                ctx.UserConnections.Add(conn);
            }
        }

        public User GetUser(Guid userID)
        {
            return ctx.Users.SingleOrDefault(x => x.ID == userID);
        }

        public List<string> GetUserNames(Guid roomID)
        {
            return ctx.UserConnections.Where(x => x.RoomID == roomID)
                .Select(x => (x.IsHost ? "(host) " : "") + 
                    ctx.Users.Single(u => u.ID == x.UserID).Name)
                .ToList();
        }

        public Guid? RemoveConnection(Guid connID)
        {

            var ret = (Guid?)Guid.Empty;
            var conn = ctx.UserConnections.Single(x => x.ID == connID);

            ctx.UserConnections.Remove(conn);

            bool removeRoom = !ctx.UserConnections.Any(x => x.RoomID == conn.RoomID);
            if (!removeRoom && conn.IsHost)
            {
                var thisRoomsUsers = ctx.UserConnections
                    .Where(x => x.RoomID == conn.RoomID)
                    .ToList();
                var newHost = thisRoomsUsers.OrderBy(x => x.CreatedDate).FirstOrDefault();
                if (newHost != null)
                {
                    newHost.IsHost = true;
                    ret = newHost.ID;
                }
                else
                {
                    //this shouldn't happen, but somehow there are now
                    //no connections left. close the room. because fuck it.
                    removeRoom = true;
                }
            }
            else
            {
                ret = null;
            }

            if (removeRoom)
            {
                ctx.Rooms.Remove(ctx.Rooms.Single(x => x.ID == conn.RoomID));
            }

            return ret;
        }

        public User GetUserByConnection(Guid connID)
        {
            var userId = ctx.UserConnections.Single(x => x.ID == connID).UserID;
            return GetUser(userId);
        }

        public void UpdateHost(Guid roomID)
        {
            var room = ctx.Rooms.Single(x => x.ID == roomID);
            if (room.Users.Any())
            {
                var firstConn = room.UserConnections.First();
                firstConn.IsHost = true;
                var otherConns = room.UserConnections.Where(x => x.ID != firstConn.ID);
                foreach (var con in otherConns)
                    con.IsHost = false;

            }
            else
            {
                room.ExpireDate = DateTime.Now.AddMinutes(15);
            }
        }
        public Guid GetHostConnectionID(Guid roomID)
        {
            var conn = ctx.UserConnections.FirstOrDefault(x => x.RoomID == roomID && x.IsHost);
            return conn == null ? Guid.Empty : conn.ID;
        }

        public void ChangeName(Guid connID, string name)
        {
            var user = GetUserByConnection(connID);
            user.Name = name;
        }

        public List<Room> GetAllRoomsUserIsIn(Guid connID)
        {
            var user = GetUserByConnection(connID);
            var conns = ctx.UserConnections.Where(x => x.UserID == user.ID);
            return ctx.Rooms.Where(x => conns.Select(y => y.RoomID).Contains(x.ID)).ToList();
        }


        public string GetUserNameOrUnique(Guid userID)
        {
            var user = GetUser(userID);
            if (user == null)
                return GetUniqueUserName();
            return user.Name;
        }

        private string GetUniqueUserName()
        {
            string format = "Anon {0}";

            string name = "";
            var random = new Random();
            var userCount = ctx.Users.Count();

            //random size scales with total room count, to make it more unlikely that 
            // you find a match when finding a unique room name
            var countMod10 = ((int)(userCount / 10));
            var randomSize = 100 * (countMod10 == 0 ? 1 : countMod10);
            if (randomSize > 10000)
                randomSize = 10000;
            randomSize--;

            if (randomSize == 420)
                format += " yolo blaze it";

            while (true)
            {
                name = string.Format(format, random.Next(randomSize));
                if (!ctx.Users.Any(x => x.Name == name))
                    break;
            }
            return name;
        }

        #endregion Users





        #region Videos



        public List<Video> GetVideos(Guid roomID)
        {
            return ctx.Videos.Where(x => x.RoomID == roomID).OrderBy(x => x.Order).ToList();
        }

        /// <summary>
        /// Doesn't currently restrict to not have dupes. I could implement this to just return distinct instead, not sure.
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="video"></param>
        public Video AddVideo(Guid roomID, Video video)
        {
            int order = 0;
            //if room already has videos, get the order. otherwise stick with 0.
            if (ctx.Videos.Any(x => x.RoomID == roomID))
                order = ctx.Videos.Where(x => x.RoomID == roomID).Max(x => x.Order);

            //don't allow dupes. - DJL 11/15/2014
            if (ctx.Videos.Any(x => x.RoomID == roomID && x.ID == video.ID))
                return null;

            video.Order = order + 1;
            video.RoomID = roomID;
            if (video.Order == 1)
                video.IsCurrent = true;
            ctx.Videos.Add(video);

            return video;
        }

        public Video GetCurrentVideo(Guid roomID)
        {
            var room = GetRoom_Internal(roomID);
            return ctx.Videos.Where(x => x.RoomID == roomID).SingleOrDefault(x => x.IsCurrent);
        }

        public void SetCurrentVideo(Guid roomID, string videoID)
        {
            videoID = YouTubeHelper.GetYouTubeID(videoID);
            var newVid = ctx.Videos.SingleOrDefault(x => x.RoomID == roomID && x.ID == videoID);
            if (newVid != null)
            {
                var currentVids = ctx.Videos.Where(x => x.RoomID == roomID && x.IsCurrent);
                foreach (var vid in currentVids) //just in case there are somehow multiple.
                    vid.IsCurrent = false;
                newVid.IsCurrent = true;
            }
        }

        public bool NextVideo(Guid roomID)
        {
            return ChangeVideoByIncrement(roomID, 1);
        }
        public bool PrevVideo(Guid roomID)
        {
            return ChangeVideoByIncrement(roomID, -1);
        }

        public bool ChangeVideoByIncrement(Guid roomID, int inc)
        {
            bool anyVidNext = true;
            var currentVid = ctx.Videos.SingleOrDefault(x => x.RoomID == roomID && x.IsCurrent);
            var nextIndex = 0;
            if (currentVid != null)
            {
                nextIndex = currentVid.Order + inc;
            }
            var nextVid = ctx.Videos.SingleOrDefault(x => x.RoomID == roomID && x.Order == nextIndex);
            if (nextVid == null)
                anyVidNext = false;
            else
            {
                nextVid.IsCurrent = true;
                currentVid.IsCurrent = false;
            }

            return anyVidNext;
        }

        public void SetLoop(Guid roomID, bool loop)
        {
            var room = GetRoom_Internal(roomID);
            room.Loop = loop;
        }

        public void ClearEmptyRooms()
        {
            var roomsToDelete = ctx.Rooms.Where(x => !x.UserConnections.Any());
            foreach (var room in roomsToDelete)
                ctx.Rooms.Remove(room);
        }

        public void RemoveRoomIfEmpty(Guid roomID)
        {
            var room = ctx.Rooms.SingleOrDefault(x => x.ID == roomID);
            if (room == null)
                return;

            ctx.Rooms.Remove(room);
        }

        #endregion Videos
    }
}