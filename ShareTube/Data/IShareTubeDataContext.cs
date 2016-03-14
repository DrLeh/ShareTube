using System;
using System.Data.Entity;

namespace ShareTube.Data
{
	public interface IShareTubeDataContext :  IDisposable
	{
		DbSet<PlayerStatus> PlayerStatuses { get; set; }
		DbSet<Room> Rooms { get; set; }
		DbSet<TrackingEntry> TrackingEntries { get; set; }
		DbSet<UserConnection> UserConnections { get; set; }
		DbSet<User> Users { get; set; }
		DbSet<Video> Videos { get; set; }

		int SaveChanges();
	}
}