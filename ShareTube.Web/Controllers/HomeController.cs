using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShareTube.Web.Models;
using ShareTube.Data;
using ShareTube.Core.ViewModels;
using ShareTube.Core.Models;
using ShareTube.Core.Extensions;

namespace ShareTube.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShareTubeDataContext _context;

        public HomeController(ShareTubeDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new RoomListViewModel();
            var rooms = _context.Rooms
                .Where(x => !x.IsPrivate)
                .OrderByDescending(x => x.CreatedDate).Take(10)
                .Select(x => new RoomListItem
                {
                    ID = x.ID,
                    Name = x.Name,
                    UserCount = x.UserConnections.Count()
                })
                .ToList();

            vm.Rooms = rooms;

            return View(vm);
        }

        [HttpPost]
        public IActionResult NewRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            var encodedID = GuidEncoder.Encode(room.ID);
            return RedirectToAction(nameof(Watch), encodedID);
        }

        [HttpGet, Route("Watch/{id}")]
        public IActionResult Watch(string id)
        {
            if (!GuidEncoder.TryDecode(id, out Guid roomID))
                throw new Exception("Invalid room code");

            //bool isCrawler = Request.Browser.Crawler;
            //if (isCrawler)
            //    return Content("No crawlser pls kthx");

            var room = _context.Rooms.FirstOrDefault(x => x.ID == roomID);
            if (room == null)
                return NotFound();
            return View(room);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
