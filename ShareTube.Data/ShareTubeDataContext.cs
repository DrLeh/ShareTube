using Microsoft.EntityFrameworkCore;
using ShareTube.Core.Models;
using ShareTube.Data.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareTube.Data
{
    public class ShareTubeDataContext : DbContext
    {
        public ShareTubeDataContext()
        { }
        public ShareTubeDataContext(DbContextOptions<ShareTubeDataContext> options)
            : base(options)
        { }

        public override int SaveChanges()
        {
            var saveTime = DateTime.UtcNow;

            var addedEntities = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is Entity)
                .Select(x => x.Entity as Entity);

            var editedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is Entity)
                .Select(x => x.Entity as Entity);

            foreach (var entry in addedEntities)
                entry.CreatedDate = saveTime;
            foreach (var entry in editedEntities)
                entry.UpdatedDate = saveTime;

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Map<PlayerStatus>(modelBuilder);
            Map<Room>(modelBuilder);
            Map<User>(modelBuilder);
            Map<UserConnection>(modelBuilder);
            Map<Video>(modelBuilder);
        }

        private void Map<T>(ModelBuilder modelBuilder)
            where T : Entity
        {
            MapFactory.Create<T>()?.Map(modelBuilder.Entity<T>());
        }

        public virtual DbSet<PlayerStatus> PlayerStatuses { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserConnection> UserConnections { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        //public virtual DbSet<TrackingEntry> TrackingEntries { get; set; }


    }
}
