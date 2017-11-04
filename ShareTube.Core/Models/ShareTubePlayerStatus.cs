using System.Linq;
using System.Web;

namespace ShareTube.Core.Models
{
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