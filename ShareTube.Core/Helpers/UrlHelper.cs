using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareTube.Core.Helpers
{
    public class UrlHelper
    {
        public static string GetYouTubeID(string s) => s.Split('/').LastOrDefault();

        public static string GetYouTubeUrlFromID(string s)
        {
            if (s.Contains("https://"))
                return s;
            return string.Format("https://youtube.com/watch?v={0}", s);
        }
    }
}
