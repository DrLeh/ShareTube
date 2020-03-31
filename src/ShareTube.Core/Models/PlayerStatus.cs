using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ShareTube.Core.Models
{
    public class PlayerStatus : Entity
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

        public ShareTubePlayerStatus ShareTubePlayerStatus => (ShareTubePlayerStatus)IDToCodeMapping[ID];

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
}