using System;
using System.ComponentModel.DataAnnotations;

namespace ShareTube.Core.Models
{
    public class UserConnection : Entity
    {
        [Key]
        public Guid ID { get; set; }
        public bool IsHost { get; set; }

        public Guid UserID { get; set; }
        public virtual User User { get; set; }

        public Guid RoomID { get; set; }
        public virtual Room Room { get; set; }
    }
}