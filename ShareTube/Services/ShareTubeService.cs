using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using ShareTube.Infrastructure;
using ShareTube.Models;
using System.Data.Entity;

namespace ShareTube.Data
{
	public class ShareTubeService : IShareTubeService, IDisposable
	{
		private IShareTubeDataContext Context;
		public ShareTubeService(IShareTubeDataContext context)
		{
			Context = context;
		}
		public ShareTubeService()
		{
			Context = new ShareTubeDataContext();
		}

		public List<RoomListItem> GetPublicRooms()
		{
			return Context.Rooms.Where(x => x.IsPrivate == false)
				.Select(x => new RoomListItem
				{
					ID = x.ID,
					Name = x.Name,
					UserCount = x.UserConnections.Count()
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
			Context.Rooms.Add(room);
			Context.SaveChanges();
			return room;
		}

		public void RemoveRoom(Guid roomID)
		{
			Context.Rooms.Remove(Context.Rooms.Single(x => x.ID == roomID));
			Context.SaveChanges();
		}

		public Room GetRoom(Guid roomID)
		{
			return GetRoom_Internal(roomID);
		}

		private Room GetRoom_Internal(Guid roomID)
		{
			return Context.Rooms.SingleOrDefault(x => x.ID == roomID);
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
				Context.SaveChanges();
			}
			return room;
		}


		public string GetUniqueRoomName()
		{
			string format = "ShareTube Room - {0}";
			string name = "";
			var random = new Random();
			var roomCount = Context.Rooms.Count();

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
				if (!Context.Rooms.Any(x => x.Name == name))
					break;
			}
			return name;
		}

		public void UpdateTime(Guid roomID, double time)
		{
			var room = GetRoom_Internal(roomID);
			room.CurrentTime = time;
			Context.SaveChanges();
		}

		public void UpdateStatus(Guid roomID, ShareTubePlayerStatus status)
		{
			var room = GetRoom_Internal(roomID);
			room.ShareTubePlayerStatus = status;
			Context.SaveChanges();
		}



		#region Users

		public void AddUser(Guid roomID, Guid userID, Guid connID, string username)
		{
			var user = Context.Users.SingleOrDefault(x => x.ID == userID);
			if (user == null)
			{
				user = new User();
				user.Name = username;
				user.ID = userID;

				Context.Users.Add(user);
			}
			if (user.Connections.SingleOrDefault(x => x.ID == connID) == null)
			{
				var conn = new UserConnection
				{
					ID = connID,
					UserID = user.ID,
					RoomID = roomID
				};
				if (!Context.UserConnections.Any(x => x.RoomID == roomID))
					conn.IsHost = true;
				Context.UserConnections.Add(conn);
			}
			Context.SaveChanges();
		}

		public User GetUser(Guid userID)
		{
			return Context.Users.SingleOrDefault(x => x.ID == userID);
		}

		public List<string> GetUserNames(Guid roomID)
		{
			return Context.UserConnections
				.Where(x => x.RoomID == roomID)
				.OrderBy(x => !x.IsHost)
				.ThenBy(x => x.User.Name)
				.Select(x => (x.IsHost ? "(host) " : "") + x.User.Name)
				.ToList();
		}

		public Guid? RemoveConnection(Guid connID)
		{

			var ret = (Guid?)Guid.Empty;
			var conn = Context.UserConnections.SingleOrDefault(x => x.ID == connID);

			Context.UserConnections.Remove(conn);
			Context.SaveChanges();


			bool removeRoom = !Context.UserConnections.Any(x => x.RoomID == conn.RoomID);
			if (!removeRoom && conn.IsHost)
			{
				var thisRoomsUsers = Context.UserConnections
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
				Context.Rooms.Remove(Context.Rooms.SingleOrDefault(x => x.ID == conn.RoomID));
			}

			Context.SaveChanges();
			return ret;
		}

		public User GetUserByConnection(Guid connID)
		{
			return Context.UserConnections.SingleOrDefault(x => x.ID == connID).User;
		}

		public void UpdateHost(Guid roomID)
		{
			var room = Context.Rooms.SingleOrDefault(x => x.ID == roomID);
			if (room.Users.Any())
			{
				var firstConn = room.UserConnections.First();
				firstConn.IsHost = true;
				var otherConns = room.UserConnections.Where(x => x.ID != firstConn.ID);
				foreach (var con in otherConns)
					con.IsHost = false;
				Context.SaveChanges();
			}
			else
			{
				room.ExpireDate = DateTime.Now.AddMinutes(15);
			}
			Context.SaveChanges();
		}
		public Guid GetHostConnectionID(Guid roomID)
		{
			var conn = Context.UserConnections.FirstOrDefault(x => x.RoomID == roomID && x.IsHost);
			return conn == null ? Guid.Empty : conn.ID;
		}

		public void ChangeName(Guid connID, string name)
		{
			var user = Context.UserConnections.SingleOrDefault(x => x.ID == connID).User;
			user.Name = name;
			Context.SaveChanges();
		}

		public List<Room> GetAllRoomsUserIsIn(Guid connID)
		{
			var user = Context.UserConnections.SingleOrDefault(x => x.ID == connID).User;
			return Context.UserConnections.Where(x => x.UserID == user.ID).Select(x => x.Room).ToList();
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
			var userCount = Context.Users.Count();

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
				if (!Context.Users.Any(x => x.Name == name))
					break;
			}
			return name;
		}

		#endregion Users





		#region Videos



		public List<Video> GetVideos(Guid roomID)
		{
			return Context.Videos.Where(x => x.RoomID == roomID).OrderBy(x => x.Order).ToList();
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
			if (Context.Videos.Any(x => x.RoomID == roomID))
				order = Context.Videos.Where(x => x.RoomID == roomID).Max(x => x.Order);

			//don't allow dupes. - DJL 11/15/2014
			if (Context.Videos.Any(x => x.RoomID == roomID && x.ID == video.ID))
				return null;

			video.Order = order + 1;
			video.RoomID = roomID;
			if (video.Order == 1)
				video.IsCurrent = true;
			Context.Videos.Add(video);
			Context.SaveChanges();
            if (video.Order == 1)
                SetCurrentVideo(roomID, video.ID);
            return video;
		}

		public Video GetCurrentVideo(Guid roomID)
		{
			return GetRoom_Internal(roomID).CurrentVideo;
		}

		public void SetCurrentVideo(Guid roomID, string videoID)
		{
			videoID = YouTubeHelper.GetYouTubeID(videoID);
			var newVid = Context.Videos.SingleOrDefault(x => x.RoomID == roomID && x.ID == videoID);
			if (newVid != null)
			{
				var currentVids = Context.Videos.Where(x => x.RoomID == roomID && x.IsCurrent);
				foreach (var vid in currentVids) //just in case there are somehow multiple.
					vid.IsCurrent = false;
				newVid.IsCurrent = true;
			}
			Context.SaveChanges();
		}

		public bool NextVideo(Guid roomID)
		{
			return ChangeVideoByIncrement(roomID, 1);
		}
		public bool PrevVideo(Guid roomID)
		{
			return ChangeVideoByIncrement(roomID, -1);
		}

		private bool ChangeVideoByIncrement(Guid roomID, int inc)
		{
			bool anyVidNext = true;
			var currentVid = Context.Videos.SingleOrDefault(x => x.RoomID == roomID && x.IsCurrent);
			var nextIndex = 0;
			if (currentVid != null)
			{
				nextIndex = currentVid.Order + inc;
			}
			var nextVid = Context.Videos.SingleOrDefault(x => x.RoomID == roomID && x.Order == nextIndex);
			if (nextVid == null)
				anyVidNext = false;
			else
			{
				nextVid.IsCurrent = true;
				currentVid.IsCurrent = false;
			}
			Context.SaveChanges();
			return anyVidNext;
		}

		public void SetLoop(Guid roomID, bool loop)
		{
			var room = GetRoom_Internal(roomID);
			room.Loop = loop;
			Context.SaveChanges();
		}

		public void ClearEmptyRooms()
		{
			var roomsToDelete = Context.Rooms.Where(x => !x.UserConnections.Any());
			Context.Rooms.RemoveRange(roomsToDelete);
			Context.SaveChanges();
		}

		public void RemoveRoomIfEmpty(Guid roomID)
		{
			var room = Context.Rooms.SingleOrDefault(x => x.ID == roomID);
			if (room == null)
				return;

			if (!room.UserConnections.Any())
			{
				Context.Rooms.Remove(room);
				Context.SaveChanges();
			}
		}

		public void Dispose()
		{
			Context.Dispose();
		}

		#endregion Videos
	}
}