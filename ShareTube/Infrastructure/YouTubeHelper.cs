using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube.Infrastructure
{
    public class YouTubeHelper
    {
        public static string GetYouTubeID(string s)
        {
            return HttpUtility.ParseQueryString(s.Split('/').Last())[0];
        }

        public static string GetYouTubeUrlFromID(string s)
        {
            if (s.Contains("https://"))
                return s;
            return string.Format("https://youtube.com/watch?v={0}", s);
        }
    }
}