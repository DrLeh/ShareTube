using Xunit;
using ShareTube.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Threading;

namespace ShareTube.Tests.Tests
{
	
	public class ShareTubeServiceTests
	{
		public class TestDataContext : IShareTubeDataContext
		{
			public TestDataContext()
			{
				PlayerStatuses = new TestDbSet<PlayerStatus>();
				Rooms = new TestDbSet<Room>();
				TrackingEntries = new TestDbSet<TrackingEntry>();
				UserConnections = new TestDbSet<UserConnection>();
				Users = new TestDbSet<User>();
				Videos = new TestDbSet<Video>();
			}

			public DbSet<PlayerStatus> PlayerStatuses { get; set; }
			public DbSet<Room> Rooms { get; set; }
			public DbSet<TrackingEntry> TrackingEntries { get; set; }
			public DbSet<UserConnection> UserConnections { get; set; }
			public DbSet<User> Users { get; set; }
			public DbSet<Video> Videos { get; set; }

			public void Dispose()
			{

			}

			public int SaveChanges()
			{
				return 1;
			}
		}

		[Fact]
		public void Can_GetPublicRooms()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				addedRoom.UserConnections = new List<UserConnection>();
				var addedRoom2 = service.AddRoom("test room");
				addedRoom2.UserConnections = new List<UserConnection>();
				var rooms = service.GetPublicRooms();
				Assert.Equal(2, rooms.Count());
			}
		}

		[Fact]
		public void Can_AddRoom()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);
				Assert.Equal(addedRoom.ID, room.ID);
			}
		}

		[Fact]
		public void Can_RemoveRoom()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);
				Assert.Equal(addedRoom.ID, room.ID);

				service.RemoveRoom(room.ID);

				var rooms = service.GetPublicRooms();
				Assert.Equal(0, rooms.Count());
			}
		}

		[Fact]
		public void Can_GetRoom()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);
				Assert.Equal(addedRoom.ID, room.ID);
			}
		}


		[Fact]
		public void Can_GetOrAddRoom()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var guid = Guid.NewGuid();
				var room = service.GetOrAddRoom(guid);
				Assert.True(room != null);
				Assert.Equal(room.ID, room.ID);
			}
		}


		[Fact]
		public void Can_GetUniqueRoomName_100_Range()
		{
			TestUniqueRoomName(100);
			TestUniqueRoomName(1000);
			//TODO: test other ranges.
		}

		private void TestUniqueRoomName(int count)
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var allRoomNamesFound = new List<string>();
				for (int i = 0; i < 100; i++)
				{
					var name = service.GetUniqueRoomName();
					service.AddRoom(name);
					allRoomNamesFound.Add(name);
				}

				Assert.False(allRoomNamesFound.GroupBy(x => x).Select(x => x.Count()).Any(x => x > 1));
			}
		}

		[Fact]
		public void Can_UpdateTime()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				service.UpdateTime(room.ID, 10);
				Assert.Equal(10, service.GetRoom(addedRoom.ID).CurrentTime);

				service.UpdateTime(room.ID, 20);
				Assert.Equal(20, service.GetRoom(addedRoom.ID).CurrentTime);
			}
		}

		[Fact]
		public void Can_UpdateStatus()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				service.UpdateStatus(room.ID, ShareTubePlayerStatus.Playing);
				Assert.Equal(ShareTubePlayerStatus.Playing, room.ShareTubePlayerStatus);

				service.UpdateStatus(room.ID, ShareTubePlayerStatus.Paused);
				Assert.Equal(ShareTubePlayerStatus.Paused, room.ShareTubePlayerStatus);
			}
		}



		[Fact]
		public void Can_AddUser()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var userID = Guid.NewGuid();
				var connID = Guid.NewGuid();
				var userName = "Some test user";

				service.AddUser(room.ID, userID, connID, userName);

				room = service.GetRoom(addedRoom.ID);
				var user = service.GetUser(userID);

				Assert.NotNull(user);
				Assert.Equal(userName, user.Name);

				Assert.Equal(1, context.UserConnections.Count());
				Assert.Equal(userID, context.UserConnections.First().UserID);
			}
		}

		[Fact]
		public void Can_GetUser()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var userID = Guid.NewGuid();
				var connID = Guid.NewGuid();
				var userName = "Some test user";

				service.AddUser(room.ID, userID, connID, userName);

				room = service.GetRoom(addedRoom.ID);
				var user = service.GetUser(userID);

				Assert.NotNull(user);
				Assert.Equal(userName, user.Name);

				Assert.Equal(1, context.UserConnections.Count());
				Assert.Equal(userID, context.UserConnections.First().UserID);
			}
		}

		[Fact]
		public void Can_GetUserNames()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var userName1 = "Some test user";
				var hostName = "(host) Some test user";
				var userName2 = "Some test user2";

				service.AddUser(room.ID, Guid.NewGuid(), Guid.NewGuid(), userName1);
				service.AddUser(room.ID, Guid.NewGuid(), Guid.NewGuid(), userName2);

				foreach (var userconn in context.UserConnections)
					userconn.User = service.GetUser(userconn.UserID);

				var names = service.GetUserNames(room.ID);
				Assert.Equal(2, names.Count());
				Assert.Equal(hostName, names[0]);
				Assert.Equal(userName2, names[1]);
			}
		}

		[Fact]
		public void Can_RemoveConnection()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var userID = Guid.NewGuid();
				var connID = Guid.NewGuid();
				var userName = "Some test user";

				service.AddUser(room.ID, userID, connID, userName);

				room = service.GetRoom(addedRoom.ID);
				var user = service.GetUser(userID);

				Assert.Equal(1, context.UserConnections.Count());
				service.RemoveConnection(connID);
				Assert.Equal(0, context.UserConnections.Count());
			}
		}

		[Fact]
		public void Can_GetUserByConnection()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var userID = Guid.NewGuid();
				var connID = Guid.NewGuid();
				var userName = "Some test user";

				service.AddUser(room.ID, userID, connID, userName);

				foreach (var userconn in context.UserConnections)
					userconn.User = service.GetUser(userconn.UserID);

				room = service.GetRoom(addedRoom.ID);
				var user = service.GetUser(userID);

				var userByConn = service.GetUserByConnection(connID);
				Assert.Equal(user, userByConn);
			}
		}

		[Fact]
		public void Can_UpdateHost()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var connID1 = Guid.NewGuid();
				var connID2 = Guid.NewGuid();

				service.AddUser(room.ID, Guid.NewGuid(), connID1, "");
				service.AddUser(room.ID, Guid.NewGuid(), connID2, "");

				foreach (var userconn in context.UserConnections)
					userconn.User = service.GetUser(userconn.UserID);

				Assert.Equal(connID1, service.GetHostConnectionID(room.ID));
				service.RemoveConnection(connID1);
				service.UpdateHost(room.ID);
				Assert.Equal(connID2, service.GetHostConnectionID(room.ID));
			}
		}

		[Fact]
		public void Can_GetHostConnectionID()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var connID1 = Guid.NewGuid();

				service.AddUser(room.ID, Guid.NewGuid(), connID1, "");
				service.AddUser(room.ID, Guid.NewGuid(), Guid.NewGuid(), "");

				foreach (var userconn in context.UserConnections)
					userconn.User = service.GetUser(userconn.UserID);

				Assert.Equal(connID1, service.GetHostConnectionID(room.ID));
			}
		}

		[Fact]
		public void Can_ChangeName()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.AddRoom("test room");
				var room = service.GetRoom(addedRoom.ID);

				room.UserConnections = new List<UserConnection>();

				var userID = Guid.NewGuid();
				var connID = Guid.NewGuid();
				var originalName = "Name 1";
				var newName = "John Jacob Jingleheimer";

				service.AddUser(room.ID, userID, connID, originalName);

				foreach (var userconn in context.UserConnections)
					userconn.User = service.GetUser(userconn.UserID);

				var user = service.GetUser(userID);
				Assert.Equal(originalName, user.Name);

				service.ChangeName(connID, newName);

				var userRenamed = service.GetUser(userID);
				Assert.Equal(newName, userRenamed.Name);
			}
		}

		[Fact]
		public void Can_GetAllRoomsUserIsIn()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room1 = service.GetRoom(service.AddRoom("Room 1").ID);
				room1.UserConnections = new List<UserConnection>();

				var room2 = service.GetRoom(service.AddRoom("Room 2").ID);
				room2.UserConnections = new List<UserConnection>();

				var userID = Guid.NewGuid();
				var connID = Guid.NewGuid();
				var connID2 = Guid.NewGuid();

				service.AddUser(room1.ID, userID, connID, "");
				service.AddUser(room2.ID, userID, connID2, "");

				foreach (var userconn in context.UserConnections)
				{
					userconn.User = service.GetUser(userconn.UserID);
					userconn.Room = service.GetRoom(userconn.RoomID);
				}

				var rooms = service.GetAllRoomsUserIsIn(connID);
				Assert.Equal(2, rooms.Count());
				Assert.NotNull(rooms.SingleOrDefault(x => x.ID == room1.ID));
				Assert.NotNull(rooms.SingleOrDefault(x => x.ID == room2.ID));
			}
		}


		[Fact]
		public void Can_GetUserNameOrUnique()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room1 = service.GetRoom(service.AddRoom("Room 1").ID);
				room1.UserConnections = new List<UserConnection>();

				var userID = Guid.NewGuid();
				var connID = Guid.NewGuid();

				var userName = "Test User";

				service.AddUser(room1.ID, userID, connID, userName);

				var name = service.GetUserNameOrUnique(userID);
				Assert.Equal(name, userName);

				service.RemoveConnection(connID);
				
				var name2 = service.GetUserNameOrUnique(Guid.NewGuid());
				Assert.NotNull(name2);
			}
		}


		[Fact]
		public void Can_GetVideos()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room = service.GetRoom(service.AddRoom("Room 1").ID);
				room.UserConnections = new List<UserConnection>();

				var video = new Video
				{
					ID = "some url"
				};
				service.AddVideo(room.ID, video);

				var videos = service.GetVideos(room.ID);
				Assert.Equal(1, videos.Count());

				service.AddVideo(room.ID, new Video { ID = "some other url" });

				videos = service.GetVideos(room.ID);
				Assert.Equal(2, videos.Count());
			}
		}

		/// <summary>
		/// Doesn't currently restrict to not have dupes. I could implement this to just return distinct instead, not sure.
		/// </summary>
		/// <param name="roomID"></param>
		/// <param name="video"></param>
		[Fact]
		public void Can_AddVideo()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room = service.GetRoom(service.AddRoom("Room 1").ID);
				room.UserConnections = new List<UserConnection>();
				
				var firstUrl = "some url";
				
				service.AddVideo(room.ID, new Video { ID = firstUrl });

				var videos = service.GetVideos(room.ID);
				Assert.Equal(1, videos.Count());

				service.AddVideo(room.ID, new Video { ID = "some other url" });

				videos = service.GetVideos(room.ID);
				Assert.Equal(2, videos.Count());

				service.AddVideo(room.ID, new Video { ID = firstUrl });
				//can't add dupes
				videos = service.GetVideos(room.ID);
				Assert.Equal(2, videos.Count());
			}
		}

		[Fact]
		public void Can_GetCurrentVideo()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room = service.GetRoom(service.AddRoom("Room 1").ID);

				var firstUrl = "some url";
				var secondUrl = "some other url";

				service.AddVideo(room.ID, new Video { ID = firstUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				var currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.AddVideo(room.ID, new Video { ID = secondUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.NextVideo(room.ID);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(secondUrl, currentVideo.ID);
			}
		}

		[Fact]
		public void Can_SetCurrentVideo()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room = service.GetRoom(service.AddRoom("Room 1").ID);

				var firstUrl = "some url";
				var secondUrl = "some other url";

				service.AddVideo(room.ID, new Video { ID = firstUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				var currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.AddVideo(room.ID, new Video { ID = secondUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.SetCurrentVideo(room.ID, secondUrl);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(secondUrl, currentVideo.ID);
			}
		}

		[Fact]
		public void Can_NextVideo()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room = service.GetRoom(service.AddRoom("Room 1").ID);

				var firstUrl = "some url";
				var secondUrl = "some other url";

				service.AddVideo(room.ID, new Video { ID = firstUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				var currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.AddVideo(room.ID, new Video { ID = secondUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.NextVideo(room.ID);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(secondUrl, currentVideo.ID);
			}
		}
		[Fact]
		public void Can_PrevVideo()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var room = service.GetRoom(service.AddRoom("Room 1").ID);

				var firstUrl = "some url";
				var secondUrl = "some other url";

				service.AddVideo(room.ID, new Video { ID = firstUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				var currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.AddVideo(room.ID, new Video { ID = secondUrl });

				room.Videos = new List<Video>();
				foreach (var vid in service.GetVideos(room.ID))
					room.Videos.Add(vid);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);

				service.SetCurrentVideo(room.ID, secondUrl);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(secondUrl, currentVideo.ID);

				service.PrevVideo(room.ID);

				currentVideo = service.GetCurrentVideo(room.ID);
				Assert.Equal(firstUrl, currentVideo.ID);
			}
		}
		
		[Fact]
		public void Can_SetLoop()
		{
			var context = new TestDataContext();
			using (var service = new ShareTubeService(context))
			{
				var addedRoom = service.GetRoom(service.AddRoom("Room 1").ID);

				var room = service.GetRoom(addedRoom.ID);

				Assert.Equal(false, room.Loop);

				service.SetLoop(addedRoom.ID, true);
				room = service.GetRoom(addedRoom.ID);

				Assert.Equal(true, room.Loop);
				service.SetLoop(addedRoom.ID, false);
				room = service.GetRoom(addedRoom.ID);

				Assert.Equal(false, room.Loop);
			}
		}

		[Fact]
		public void Can_ClearEmptyRooms()
		{
			{
				var context = new TestDataContext();
				using (var service = new ShareTubeService(context))
				{
					var room = service.GetRoom(service.AddRoom("Room 1").ID);
					room.UserConnections = new List<UserConnection>();

					service.ClearEmptyRooms();

					var roomList = service.GetPublicRooms();
					Assert.Equal(0, roomList.Count());
				}
			}
			{
				var context = new TestDataContext();
				using (var service = new ShareTubeService(context))
				{
					var room = service.GetRoom(service.AddRoom("Room 1").ID);
					room.UserConnections = new List<UserConnection>();
					room.UserConnections.Add(new UserConnection { UserID = Guid.NewGuid(), ID = Guid.NewGuid() });

					service.ClearEmptyRooms();

					var roomList = service.GetPublicRooms();
					Assert.Equal(1, roomList.Count());
				}
			}
		}

		[Fact]
		public void Can_RemoveRoomIfEmpty()
		{
			{
				var context = new TestDataContext();
				using (var service = new ShareTubeService(context))
				{
					var room = service.GetRoom(service.AddRoom("Room 1").ID);
					room.UserConnections = new List<UserConnection>();

					service.RemoveRoomIfEmpty(room.ID);

					var roomList = service.GetPublicRooms();
					Assert.Equal(0, roomList.Count());
				}
			}
			{
				var context = new TestDataContext();
				using (var service = new ShareTubeService(context))
				{
					var room = service.GetRoom(service.AddRoom("Room 1").ID);
					room.UserConnections = new List<UserConnection>();
					room.UserConnections.Add(new UserConnection { UserID = Guid.NewGuid(), ID = Guid.NewGuid() });

					service.RemoveRoomIfEmpty(room.ID);

					var roomList = service.GetPublicRooms();
					Assert.Equal(1, roomList.Count());
				}
			}
		}

	}
}
