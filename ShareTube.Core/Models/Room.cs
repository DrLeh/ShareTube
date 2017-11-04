using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ShareTube.Core.Models
{
    public class Room : Entity
    {

        public Guid ID { get; set; } = Guid.NewGuid();
        [MinLength(1, ErrorMessage = "Room name must be at least one character in length")]
        [Required(ErrorMessage = "Room Name is required")]
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool Loop { get; set; }
        public double CurrentTime { get; set; }

        public DateTime? ExpireDate { get; set; }


        public virtual List<Video> Videos { get; set; } = new List<Video>();
        public virtual List<UserConnection> UserConnections { get; set; } = new List<UserConnection>();

        public int PlayerStatusID { get; set; } = 1;
        public virtual PlayerStatus Status { get; set; }


        public ShareTubePlayerStatus ShareTubePlayerStatus
        {
            get => (ShareTubePlayerStatus)PlayerStatus.IDToCodeMapping[PlayerStatusID];
            set => PlayerStatusID = PlayerStatus.IDToCodeMapping.Single(x => x.Value == (int)value).Key;
        }

        public string CurrentVideoID { get { return CurrentVideo?.ID; } }
        public Video CurrentVideo { get { return Videos.SingleOrDefault(x => x.IsCurrent); } }
        public IEnumerable<User> Users => UserConnections.Select(x => x.User);

        public UserConnection HostConnection => UserConnections.FirstOrDefault(x => x.IsHost);
    }
}