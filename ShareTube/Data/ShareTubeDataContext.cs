using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShareTube.Data
{
    public class ShareTubeDataContext : DbContext, IShareTubeDataContext
    {
        public ShareTubeDataContext()
            : base("ShareTube")
        {
            Database.SetInitializer(new ShareTubeDataInitializer());
        }

        public override int SaveChanges()
        {
            var saveTime = DateTime.UtcNow;

            var addedEntities = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is ShareTubeEntity)
                .Select(x => x.Entity as ShareTubeEntity);

            var editedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is ShareTubeEntity)
                .Select(x => x.Entity as ShareTubeEntity);

            foreach (var entry in addedEntities)
                entry.CreatedDate = saveTime;
            foreach (var entry in editedEntities)
                entry.UpdatedDate = saveTime;

            return base.SaveChanges();

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<PlayerStatus> PlayerStatuses { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserConnection> UserConnections { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<TrackingEntry> TrackingEntries { get; set; }
    }

    public class ShareTubeDataInitializer : IDatabaseInitializer<ShareTubeDataContext>
    {
        private bool AlwaysRecreate = true;

        public void InitializeDatabase(ShareTubeDataContext context)
        {
            if (AlwaysRecreate)
            {
                var trackingEntries = new List<TrackingEntry>();
                if (context.Database.Exists())
                {
                    //trackingEntries = context.TrackingEntries.ToList();
                    //context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                    //    "ALTER DATABASE [" 
                    //    + context.Database.Connection.Database
                    //    + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    //context.Database.Delete();
                }
                else
                    context.Database.Create();
                Seed(context);
                SeedTracking(context, trackingEntries);
                context.SaveChanges();
            }
        }

        private void Seed(ShareTubeDataContext ctx)
        {
            //ctx.Database.ExecuteSqlCommand("create function ConnectionHostCount() returns int as begin declare @ret int = 0; select @ret = count(id) from Userconnections group by roomid, ishost; return @ret end");
            //ctx.Database.ExecuteSqlCommand("ALTER TABLE UserConnections ADD CONSTRAINT CK_Single_Host CHECK (dbo.ConnectionHostCount() <= 1)");

            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.UnStarted));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Ended));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Playing));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Paused));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Buffering));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Cued));
        }
        private void SeedTracking(ShareTubeDataContext ctx, IEnumerable<TrackingEntry> trackings)
        {
            ctx.TrackingEntries.AddRange(trackings);
        }
    }
}