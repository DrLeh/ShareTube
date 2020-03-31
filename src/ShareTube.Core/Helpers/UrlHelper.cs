using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareTube.Core.Helpers
{
    public static class UrlHelper
    {
        public static string GetYouTubeID(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            var splits = s.Replace("?", "&").Split('&').Where(x => x.Contains('=')).ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);
            return splits["v"];
        }

        public static string GetYouTubeUrlFromID(string s)
        {
            if (s.Contains("https://"))
                return s;
            return string.Format("https://youtube.com/watch?v={0}", s);
        }
    }
}
