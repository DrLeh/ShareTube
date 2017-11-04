using Microsoft.AspNetCore.Mvc;
using ShareTube.Core.Extensions;
using ShareTube.Core.Models;
using ShareTube.Core.ViewModels;
using ShareTube.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareTube.Web.Controllers
{
    [Route("api/v1/rooms")]
    public class RoomController : Controller
    {
        private readonly ShareTubeDataContext _context;

        public RoomController(ShareTubeDataContext context)
        {
            _context = context;
        }

        [HttpGet, Route("")]
        public IEnumerable<RoomSearchResult> Get()
        {
            return _context.Rooms
                .Where(x => !x.IsPrivate)
                .OrderByDescending(x => x.CreatedDate).Take(10)
                .Select(x => new RoomSearchResult
                {
                    Id = x.IdEncoded,
                    Name = x.Name,
                    UserCount = x.UserConnections.Count()
                })
                .ToList();
        }

        [HttpGet, Route("{id}")]
        public Room GetById(string id)
        {
            var room = _context.Rooms.FirstOrDefault(x => x.ID == id.DecodeGuid());
            //throw?
            return room;
        }

        [HttpPost, Route("")]
        public Room NewRoom([FromBody] Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            return room;
        }
    }
}
