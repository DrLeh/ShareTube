using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShareTube.Models;
using HtmlAgilityPack;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using ShareTube.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using ShareTube.Data;
using Microsoft.Practices.Unity;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace ShareTube.Controllers
{
	public partial class ShareTubeController : Controller
	{
		public ShareTubeController() { }


		public virtual ActionResult Index()
		{
			using (var Service = new ShareTubeService())
			{
				ViewBag.NewRoom = new Room { Name = Service.GetUniqueRoomName() };
				var vm = new RoomListViewModel() { Rooms = Service.GetPublicRooms(), ShowCreate = false };
				return View(vm);
			}
		}

		public virtual ActionResult NewRoom()
		{
			return View();
		}

		[HttpPost]
		public async virtual Task<ActionResult> NewRoom(Room room)
		{
			using (var Service = new ShareTubeService())
			{
				var room2 = Service.AddRoom(room.Name);
				var encodedID = GuidEncoder.Encode(room2.ID);
				//error here. this method needs to be awaited somehow.
				RemoveEmptyRoomsAsync(room2.ID, 10000);
				return RedirectToAction(MVC.ShareTube.Watch(encodedID));
			}
		}

		public virtual ActionResult WatchLoginCallback(string id, string loginFromConnectionID)
		{
			TempData["ReplaceConnectionID"] = loginFromConnectionID;
			return RedirectToAction(MVC.ShareTube.Watch(id));
		}


		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

        [Dependency]
        public ICookieHelper CookieHelper { get; set; }

        public virtual async Task<ActionResult> Watch(string id)
		{
			bool isCrawler = Request.Browser.Crawler;
			if (isCrawler)
				return Content("No crawlser pls kthx");

			bool createRoom = false;
			Room room = null;

			if (string.IsNullOrWhiteSpace(id))
				createRoom = true;
			Guid roomID = Guid.Empty;
			if (!GuidEncoder.TryDecode(id, out roomID))
				createRoom = true;

			using (var Service = new ShareTubeService())
			{
				room = Service.GetRoom(roomID);
				if (room == null)
					createRoom = true;


				if (createRoom)
				{
					room = Service.AddRoom();
					var encodedID = GuidEncoder.Encode(room.ID);
					return RedirectToAction(MVC.ShareTube.Watch(encodedID));
				}


				var vm = new RoomListViewModel() { Rooms = Service.GetPublicRooms().Take(10), ShowCreate = true };
				ViewBag.OtherRooms = vm;
				if (TempData["ReplaceConnectionID"] != null)
					ViewBag.ReplaceConnectionID = TempData["ReplaceConnectionID"];

				string userName = null;
				if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity != null)
				{
					userName = HttpContext.User.Identity.Name;
					string token = null;
					var googleTokenCookie = Request.Cookies["googletoken"];

					//var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
					var loginInfo = await AuthenticationManager_GetExternalLoginInfoAsync_Workaround();
					if (loginInfo != null)
					{
						if (loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type.Equals("urn:tokens:google:accesstoken")) != null)
						{
							token = loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type.Equals("urn:tokens:google:accesstoken")).ToString();
						}
					}
					else if (googleTokenCookie != null)
					{
						token = googleTokenCookie["token"];
					}
					else if (Session["googletoken"] != null)
					{
						token = (string)Session["googletoken"];
					}

					ViewBag.GoogleAccessToken = token;
				}

				var userID = CookieHelper.GetOrAddUserID();
				userName = Service.GetUserNameOrUnique(userID);
				ViewBag.Username = userName;

				if (createRoom)
				{
					RemoveEmptyRoomsAsync(roomID, 10000);
				}

				return View(room);
			}
		}


		private async Task<ExternalLoginInfo> AuthenticationManager_GetExternalLoginInfoAsync_Workaround()
		{
			ExternalLoginInfo loginInfo = null;

			var result = await AuthenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);

			if (result != null && result.Identity != null)
			{
				var idClaim = result.Identity.FindFirst(ClaimTypes.NameIdentifier);
				if (idClaim != null)
				{
					loginInfo = new ExternalLoginInfo()
					{
						DefaultUserName = result.Identity.Name == null ? "" : result.Identity.Name.Replace(" ", "")
						,
						Login = new UserLoginInfo(idClaim.Issuer, idClaim.Value)
					};
				}
			}
			return loginInfo;
		}
		

		public virtual ActionResult WatchYT(string v)
		{
			bool isCrawler = Request.Browser.Crawler;
			if (isCrawler)
				return Content("No crawlser pls kthx");


			using (var Service = new ShareTubeService())
			{
				var room = Service.AddRoom();

				//don't add a vid if not valid.
				if (!string.IsNullOrEmpty(v))
				{
					Guid roomID = Guid.Empty;
					var baseUrl = "https://youtube.com/watch";
					var queryString = "?v=" + v;
					var fullUrl = baseUrl + queryString;


					//TODO: add more checks here to ensure it loaded an actual video.
					//TODO: this needs to change to use the API to get the title instead of loading the document....
					

					var videoID = v;
					var format = "https://gdata.youtube.com/feeds/api/videos/{0}?v=2";


					var request = WebRequest.Create(string.Format(format, videoID));
					var response = (HttpWebResponse)request.GetResponse();
					var xml = "";
					using (var sr = new StreamReader(response.GetResponseStream()))
						xml = sr.ReadToEnd();


					var title = new XDocument(xml).Element("entry").Element("title").Value;
					

					Service.AddVideo(room.ID, new Data.Video
					{
						ID = videoID ,
						Author = "author",
						Title = title
					});
				}

				return RedirectToAction(MVC.ShareTube.Watch(GuidEncoder.Encode(room.ID)));
			}
		}


		[ChildActionOnly]
		public virtual PartialViewResult RoomList()
		{
			using (var Service = new ShareTubeService())
			{
				var vm = new RoomListViewModel() { Rooms = Service.GetPublicRooms(), ShowCreate = false };
				return PartialView(vm);
			}
		}

		[HttpPost]
		public virtual ActionResult RoomListAjax()
		{
			using (var Service = new ShareTubeService())
			{
				var vm = new RoomListViewModel() { Rooms = Service.GetPublicRooms(), ShowCreate = true };
				return PartialView("RoomList", vm);
			}
		}

		public virtual ActionResult PlaylistEntry(Data.Video vm)
		{
			return View(vm);
		}

		public virtual ActionResult SearchResults(List<Data.Video> vms)
		{
			return View(vms);
		}

		public virtual ActionResult ClearEmptyRooms()
		{
			using (var Service = new ShareTubeService())
			{
				Service.ClearEmptyRooms();
				return RedirectToAction(MVC.ShareTube.Index());
			}
		}

		public void RemoveEmptyRoomsAsync(Guid roomID, int msTimeout)
		{
			Task.Run(() =>
			{
				Thread.Sleep(msTimeout);
				using (var Service = new ShareTubeService())
				{
					Service.RemoveRoomIfEmpty(roomID);
				}
			});
		}
	}
}
