using System.Web;
using System.Web.Optimization;

namespace Links
{
    public static partial class Bundles
    {
        public static partial class Scripts
        {
            public static string jquery = "~/bundles/jquery";
            public static string jqueryval = "~/bundles/jqueryval";
            public static string jqueryui = "~/bundles/jqueryui";
            public static string modernizr = "~/bundles/modernizr";
            public static string bootstrap = "~/bundles/bootstrap";
            public static string jquerysignalr = "~/bundles/jquerysignalr";
            public static string notify = "~/bundles/notify";
            public static string unobtrusiveajax = "~/bundles/unobtrusive-ajax";
            public static string sharetube = "~/bundles/sharetube";
        }
        public static partial class Styles
        {
            public static string css = "~/Content/css";
        }
    }
}
namespace ShareTube
{

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle(Links.Bundles.Styles.css).Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.jquery).Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.jqueryval).Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.jqueryui).Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.modernizr).Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.bootstrap).Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.jquerysignalr).Include(
                        "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.notify).Include(
                        "~/Scripts/notify.min.js"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.unobtrusiveajax).Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle(Links.Bundles.Scripts.sharetube).Include(
                        "~/Scripts/WatchTS.js"
                        ));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else 
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
