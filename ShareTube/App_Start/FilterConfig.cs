using ShareTube.Filter;
using System.Web;
using System.Web.Mvc;

namespace ShareTube
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TrackingAttribute(true));
        }
    }
}
