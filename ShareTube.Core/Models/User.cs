using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareTube.Core.Models
{
    public class User : Entity
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
}