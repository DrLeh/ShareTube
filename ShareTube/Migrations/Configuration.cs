namespace ShareTube.Migrations
{
    using ShareTube.Data;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShareTube.Data.ShareTubeDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShareTube.Data.ShareTubeDataContext ctx)
        {
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.UnStarted));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Ended));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Playing));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Paused));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Buffering));
            ctx.PlayerStatuses.Add(new PlayerStatus(ShareTubePlayerStatus.Cued));
        }
    }
}
