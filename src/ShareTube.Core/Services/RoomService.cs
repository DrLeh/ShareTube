using ShareTube.Core.Data;
using ShareTube.Core.Loaders;
using ShareTube.Core.Models;
using ShareTube.Core.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Services
{
    public interface IRoomService
    {
        List<Room> GetPublicRooms();
        Room AddRoom(string? name = null);
        //void AddUser(Guid roomID, Guid userID, Guid connID, string username);
        //void ChangeName(Guid connID, string name);
        //void ClearEmptyRooms();
        //void RemoveRoomIfEmpty(Guid roomID);
        //List<Room> GetAllRoomsUserIsIn(Guid connID);
        //Guid GetHostConnectionID(Guid roomID);
        //Room GetOrAddRoom(Guid roomID);
        //Room GetRoom(Guid roomID);
        //string GetUniqueRoomName();
        //User GetUser(Guid userID);
        //User GetUserByConnection(Guid connID);
        //string GetUserNameOrUnique(Guid userID);
        //List<string> GetUserNames(Guid roomID);
        //Guid? RemoveConnection(Guid connID);
        //void RemoveRoom(Guid roomID);
        //void UpdateHost(Guid roomID);
    }

    public class RoomService : IRoomService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IRoomLoader _roomLoader;
        private readonly IRoomNameGenerator _roomNameGenerator;

        public RoomService(IDataAccess dataAccess, IRoomLoader roomLoader, IRoomNameGenerator roomNameGenerator)
        {
            _dataAccess = dataAccess;
            _roomLoader = roomLoader;
            _roomNameGenerator = roomNameGenerator;
        }


        public List<Room> GetPublicRooms()
        {
            return _roomLoader.GetPublicRooms();
        }

        public Room AddRoom(string? name = null)
        {
            var trans = _dataAccess.CreateTransaction();
            if (name == null)
                name = _roomNameGenerator.GenerateRoomName();

            //todo: validate room name
            var room = new Room
            {
                Name = name
            };
            trans.Add(room);
            trans.Commit();
            return room;
        }

        //public void RemoveRoom(Guid roomID)
        //{
        //    Context.Rooms.Remove(Context.Rooms.Single(x => x.ID == roomID));
        //    Context.SaveChanges();
        //}

        //public Room GetRoom(Guid roomID)
        //{
        //    return GetRoom_Internal(roomID);
        //}

        //private Room GetRoom_Internal(Guid roomID)
        //{
        //    return Context.Rooms.SingleOrDefault(x => x.ID == roomID);
        //}

        //public Room GetOrAddRoom(Guid roomID)
        //{
        //    var room = GetRoom_Internal(roomID);
        //    if (room == null)
        //    {
        //        room = new Room()
        //        {
        //            Name = GetUniqueRoomName(),
        //        };
        //        Context.SaveChanges();
        //    }
        //    return room;
        //}

        //public void UpdateTime(Guid roomID, double time)
        //{
        //    var room = GetRoom_Internal(roomID);
        //    room.CurrentTime = time;
        //    Context.SaveChanges();
        //}

        //public void UpdateStatus(Guid roomID, ShareTubePlayerStatus status)
        //{
        //    var room = GetRoom_Internal(roomID);
        //    room.ShareTubePlayerStatus = status;
        //    Context.SaveChanges();
        //}



        //#region Users

        //public void AddUser(Guid roomID, Guid userID, Guid connID, string username)
        //{
        //    var user = Context.Users.SingleOrDefault(x => x.ID == userID);
        //    if (user == null)
        //    {
        //        user = new User();
        //        user.Name = username;
        //        user.ID = userID;

        //        Context.Users.Add(user);
        //    }
        //    if (user.Connections.SingleOrDefault(x => x.ID == connID) == null)
        //    {
        //        var conn = new UserConnection
        //        {
        //            ID = connID,
        //            UserID = user.ID,
        //            RoomID = roomID
        //        };
        //        if (!Context.UserConnections.Any(x => x.RoomID == roomID))
        //            conn.IsHost = true;
        //        Context.UserConnections.Add(conn);
        //    }
        //    Context.SaveChanges();
        //}

        //public User GetUser(Guid userID)
        //{
        //    return Context.Users.SingleOrDefault(x => x.ID == userID);
        //}

        //public List<string> GetUserNames(Guid roomID)
        //{
        //    return Context.UserConnections
        //        .Where(x => x.RoomID == roomID)
        //        .OrderBy(x => !x.IsHost)
        //        .ThenBy(x => x.User.Name)
        //        .Select(x => (x.IsHost ? "(host) " : "") + x.User.Name)
        //        .ToList();
        //}

        //public Guid? RemoveConnection(Guid connID)
        //{

        //    var ret = (Guid?)Guid.Empty;
        //    var conn = Context.UserConnections.SingleOrDefault(x => x.ID == connID);

        //    Context.UserConnections.Remove(conn);
        //    Context.SaveChanges();


        //    bool removeRoom = !Context.UserConnections.Any(x => x.RoomID == conn.RoomID);
        //    if (!removeRoom && conn.IsHost)
        //    {
        //        var thisRoomsUsers = Context.UserConnections
        //            .Where(x => x.RoomID == conn.RoomID)
        //            .ToList();
        //        var newHost = thisRoomsUsers.OrderBy(x => x.CreatedDate).FirstOrDefault();
        //        if (newHost != null)
        //        {
        //            newHost.IsHost = true;
        //            ret = newHost.ID;
        //        }
        //        else
        //        {
        //            //this shouldn't happen, but somehow there are now
        //            //no connections left. close the room. because fuck it.
        //            removeRoom = true;
        //        }
        //    }
        //    else
        //    {
        //        ret = null;
        //    }

        //    if (removeRoom)
        //    {
        //        Context.Rooms.Remove(Context.Rooms.SingleOrDefault(x => x.ID == conn.RoomID));
        //    }

        //    Context.SaveChanges();
        //    return ret;
        //}

        //public User GetUserByConnection(Guid connID)
        //{
        //    return Context.UserConnections.SingleOrDefault(x => x.ID == connID).User;
        //}

        //public void UpdateHost(Guid roomID)
        //{
        //    var room = Context.Rooms.SingleOrDefault(x => x.ID == roomID);
        //    if (room.Users.Any())
        //    {
        //        var firstConn = room.UserConnections.First();
        //        firstConn.IsHost = true;
        //        var otherConns = room.UserConnections.Where(x => x.ID != firstConn.ID);
        //        foreach (var con in otherConns)
        //            con.IsHost = false;
        //        Context.SaveChanges();
        //    }
        //    else
        //    {
        //        room.ExpireDate = DateTime.Now.AddMinutes(15);
        //    }
        //    Context.SaveChanges();
        //}
        //public Guid GetHostConnectionID(Guid roomID)
        //{
        //    var conn = Context.UserConnections.FirstOrDefault(x => x.RoomID == roomID && x.IsHost);
        //    return conn == null ? Guid.Empty : conn.ID;
        //}

        //public void ChangeName(Guid connID, string name)
        //{
        //    var user = Context.UserConnections.SingleOrDefault(x => x.ID == connID).User;
        //    user.Name = name;
        //    Context.SaveChanges();
        //}

        //public List<Room> GetAllRoomsUserIsIn(Guid connID)
        //{
        //    var user = Context.UserConnections.SingleOrDefault(x => x.ID == connID).User;
        //    return Context.UserConnections.Where(x => x.UserID == user.ID).Select(x => x.Room).ToList();
        //}


        //public string GetUserNameOrUnique(Guid userID)
        //{
        //    var user = GetUser(userID);
        //    if (user == null)
        //        return GetUniqueUserName();
        //    return user.Name;
        //}

        //private string GetUniqueUserName()
        //{
        //    string format = "Anon {0}";

        //    string name = "";
        //    var random = new Random();
        //    var userCount = Context.Users.Count();

        //    //random size scales with total room count, to make it more unlikely that 
        //    // you find a match when finding a unique room name
        //    var countMod10 = ((int)(userCount / 10));
        //    var randomSize = 100 * (countMod10 == 0 ? 1 : countMod10);
        //    if (randomSize > 10000)
        //        randomSize = 10000;
        //    randomSize--;

        //    if (randomSize == 420)
        //        format += " yolo blaze it";

        //    while (true)
        //    {
        //        name = string.Format(format, random.Next(randomSize));
        //        if (!Context.Users.Any(x => x.Name == name))
        //            break;
        //    }
        //    return name;
        //}





    }
}
