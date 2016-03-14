using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube
{
    public static class Constants
    {
        public const string ShareTubeRoomApplicationKey = "{02F14C49-045A-43E0-A357-65AAF1EF64FE}";
        public const string UsernameCookieName = "ShareTubeUserName";
        public const string UserIDCookieName = "ShareTubeUserID";

        public const int MAX_USER_MESSAGE_LENGTH = 140;

        public static class MemoryKeys
        {
            public const string Room = "{2FA81F15-BCF5-4E65-A242-1E993AE32BC8}";
            public const string PlayerStatus = "{70C74CF0-70D7-43FD-A655-55472A617F0D}";
            public const string User = "{0473EB12-58C7-4768-919B-C3220C83A7A4}";
            public const string UserConnection = "{CCB4CDB1-62C1-4DD1-9AC3-DBFAA20F4BBB}";
            public const string Video = "{2A260184-5CEA-4054-ACA3-6A1D6C4A82B0}";
        }
    }
}