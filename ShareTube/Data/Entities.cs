using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShareTube.Infrastructure;
using ShareTube.Models;
using System.Web.Script.Serialization;

namespace ShareTube.Data
{
    public class ShareTubeEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class Room : ShareTubeEntity
    {
        #region Mapped Fields

        public Guid ID { get; set; } = Guid.NewGuid();
        [MinLength(1, ErrorMessage = "Room name must be at least one character in length")]
        [Required(ErrorMessage = "Room Name is required")]
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool Loop { get; set; }
        public double CurrentTime { get; set; }

        public DateTime? ExpireDate { get; set; }


        public virtual List<Video> Videos { get; set; }
        public virtual List<UserConnection> UserConnections { get; set; }
        
        public int PlayerStatusID { get; set; }  = 1;
        public virtual PlayerStatus Status { get; set; }

        #endregion Mapped Fields

        public ShareTubePlayerStatus ShareTubePlayerStatus
        {
            get { return (ShareTubePlayerStatus)PlayerStatus.IDToCodeMapping[PlayerStatusID]; }
            set { PlayerStatusID = PlayerStatus.IDToCodeMapping.Single(x => x.Value == (int)value).Key; }
        }

        public string CurrentVideoID { get { return CurrentVideo.ID; } }
        public Video CurrentVideo { get { return Videos.SingleOrDefault(x => x.IsCurrent); } }
        public IEnumerable<User> Users
        {
            get
            {
                return UserConnections.Select(x => x.User);
            }
        }

        public UserConnection HostConnection
        {
            get
            {
                return UserConnections.First(x => x.IsHost);
            }
        }
    }

    public class Video : ShareTubeEntity
    {
        [Key, Column(Order = 1)]
        public string ID { get; set; }

        [Key, Column(Order = 2)]
        public Guid RoomID { get; set; }

		[ScriptIgnore]
		public virtual Room Room { get; set; }

        [Key, Column(Order = 3)]
        public int Order { get; set; }

        public string Author { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public TimeSpan Length { get; set; }
        public bool IsCurrent { get; set; }

		public string LengthString
        {
            get
            {
                var min = Length.Minutes + (60 * Length.Hours);
                return min + ":" + Length.Seconds;
            }
        }

        [NotMapped]
        public string YouTubeUrl
        {
            get
            {
                return YouTubeHelper.GetYouTubeUrlFromID(ID);
            }
            set
            {
                ID = YouTubeHelper.GetYouTubeID(value);
            }
        }
    }

    public class User : ShareTubeEntity
    {
        public User()
        {
            Connections = new List<UserConnection>();
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }

        public virtual List<UserConnection> Connections { get; set; }
    }

    public class UserConnection : ShareTubeEntity
    {
        public Guid ID { get; set; }
        public bool IsHost { get; set; }

        public Guid UserID { get; set; }
        public virtual User User { get; set; }

        public Guid RoomID { get; set; }
        public virtual Room Room { get; set; }
    }

    public class PlayerStatus : ShareTubeEntity
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(40)]
        public string Description { get; set; }

        public PlayerStatus(ShareTubePlayerStatus status)
        {
            ID = IDToCodeMapping.ToDictionary(x => x.Value, x => x.Key)[(int)status];
            Description = status.ToString();
        }

        [NotMapped]
        public ShareTubePlayerStatus ShareTubePlayerStatus
        {
            get
            {
                return (ShareTubePlayerStatus)IDToCodeMapping[ID];
            }
        }

        [NotMapped]
        public static Dictionary<int, int> IDToCodeMapping = new Dictionary<int, int>
        {
           {1 , -1},
           {2 , 0},
           {3 , 1},
           {4 , 2},
           {5 , 3},
           {6 , 5},
        };

    }

    public enum ShareTubePlayerStatus
    {
        UnStarted = -1,
        Ended = 0,
        Playing = 1,
        Paused = 2,
        Buffering = 3,
        Cued = 5,
    }
}