using Microsoft.Practices.Unity;
using ShareTube.Filter;
using ShareTube.Models.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareTube.Controllers
{
    public partial class TrackingController : Controller
    {

        [Dependency]
        public ITrackingService TrackingService { get; set; }

        public int PageSize = 50;

        [Tracking(false)]
        public virtual ActionResult Index()
        {
            var vm = new TrackingListViewModel { PageNumber = 1 };
            return View(vm);
        }

        [HttpPost]
        [Tracking(false)]
        public virtual PartialViewResult _TrackingList(TrackingListViewModel vm)
        {
            if (vm != null)
            {
                if (vm.PageNumber <= 1)
                    vm.PageNumber = 1;
                var trackings = TrackingService.GetTrackings((vm.PageNumber -1)* PageSize, PageSize);
                return PartialView(trackings);
            }
            return PartialView(null);
        }
    }
}