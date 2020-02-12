namespace ShareTube.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 40),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        Loop = c.Boolean(nullable: false),
                        CurrentTime = c.Double(nullable: false),
                        ExpireDate = c.DateTime(),
                        PlayerStatusID = c.Int(nullable: false),
                        ShareTubePlayerStatus = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PlayerStatus", t => t.PlayerStatusID, cascadeDelete: true)
                .Index(t => t.PlayerStatusID);
            
            CreateTable(
                "dbo.UserConnections",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IsHost = c.Boolean(nullable: false),
                        UserID = c.Guid(nullable: false),
                        RoomID = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.RoomID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        RoomID = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Author = c.String(),
                        Title = c.String(),
                        ThumbnailUrl = c.String(),
                        Length = c.Time(nullable: false, precision: 7),
                        IsCurrent = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.ID, t.RoomID, t.Order })
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .Index(t => t.RoomID);
            
            CreateTable(
                "dbo.TrackingEntries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RequestedUrl = c.String(),
                        UserAgentString = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        IPAddress = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);


        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "RoomID", "dbo.Rooms");
            DropForeignKey("dbo.UserConnections", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserConnections", "RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "PlayerStatusID", "dbo.PlayerStatus");
            DropIndex("dbo.Videos", new[] { "RoomID" });
            DropIndex("dbo.UserConnections", new[] { "RoomID" });
            DropIndex("dbo.UserConnections", new[] { "UserID" });
            DropIndex("dbo.Rooms", new[] { "PlayerStatusID" });
            DropTable("dbo.TrackingEntries");
            DropTable("dbo.Videos");
            DropTable("dbo.Users");
            DropTable("dbo.UserConnections");
            DropTable("dbo.Rooms");
            DropTable("dbo.PlayerStatus");
        }
    }
}
