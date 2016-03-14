
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube.Infrastructure
{
    public enum MessageType
    {
        UserJoin = 1,
        UserLeave = 2,
        UserMessage = 3,
        VideoQueue = 4,
    }
}