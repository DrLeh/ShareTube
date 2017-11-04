using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareTube.Core.Extensions;
using ShareTube.Core.Helpers;
using ShareTube.Core.Models;
using ShareTube.Core.ViewModels;
using ShareTube.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTube.Web.Controllers
{
    [Route("api/v1/videos")]
    public class VideoController : Controller
    {
        private readonly ShareTubeDataContext _context;

        public VideoController(ShareTubeDataContext context)
        {
            _context = context;
        }

        private Video TestVideo(string roomId) => new Video
        {
            Title = "test",
            //YouTubeUrl = "https://www.youtube.com/watch?v=QR_8ck8veU0",
            ID = "QR_8ck8veU0",
            RoomID = roomId.DecodeGuid(),
            Author = "Matt and Tom"
        };

        [HttpGet, Route("{roomId}")]
        public IEnumerable<Video> Get(string roomId)
        {
            //return new[] { TestVideo(roomId) };
            return _context.Videos
                .Where(x => x.RoomID == roomId.DecodeGuid())
                .OrderBy(x => x.Order).Take(10)
                .ToList();
        }

        [HttpGet, Route("{roomId}/current")]
        public Video Current(string roomId)
        {
            //return TestVideo(roomId);
            var v = _context.Rooms.Include(x => x.Videos).First(x => x.ID == roomId.DecodeGuid()).CurrentVideo;
            return Video.CopyFrom(v);
        }

        [HttpPost, Route("{roomId}/current")]
        public Video Current(string roomId, [FromBody] Video video)
        {
            var order = _context.Videos.Where(x => x.RoomID == roomId.DecodeGuid()).Max(x => x.Order);
            var current = _context.Videos.Where(x => x.RoomID == roomId.DecodeGuid() && x.IsCurrent).ToList();
            foreach (var ex in current)
                ex.IsCurrent = false;
            var newCurrent = _context.Videos.Where(x => x.RoomID == roomId.DecodeGuid() && x.ID == video.ID && x.Order == video.Order).FirstOrDefault();
            newCurrent.IsCurrent = true;
            _context.SaveChanges();
            return Video.CopyFrom(newCurrent);
        }

        [HttpPost, Route("{roomId}")]
        public Video Enqueue(string roomId, [FromBody] Video video)
        {
            var order = _context.Videos.Where(x => x.RoomID == roomId.DecodeGuid()).Max(x => x.Order);
            video.RoomID = roomId.DecodeGuid();
            video.Order = order + 1;
            _context.Videos.Add(video);
            _context.SaveChanges();
            return Video.CopyFrom(video);
        }
    }
}
