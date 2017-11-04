using ShareTube.Core.Helpers;
using System;
using Xunit;

namespace ShareTube.Core.Test
{
    public class UrlHelperTest
    {
        [Fact]
        public void CanParseYoutubeId()
        {
            var url = "https://www.youtube.com/watch?v=J3MaBTVmcsE&feature=youtu.be&t=1m12s";

            var result = UrlHelper.GetYouTubeID(url);
            Assert.Equal("J3MaBTVmcsE", result);
        }
    }
}
