using System.Web;
using System.Web.Optimization;

namespace OnlineStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            

          
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //bundles.Add(new ScriptBundle("~/bundles/JavaScript").Include(
            //    "~/bower_components/respond/dest/respond.min.js",
            //    "~/bower_components/Flot/excanvas.min.js",
            //    "~/bower_components/jquery-1.x/dist/jquery.min.js",
            //    "~/bower_components/jquery-ui/jquery-ui.min.js",
            //    "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
            //     "~/bower_components/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
            //     "~/bower_components/iCheck/icheck.min.js",
            //     "~/bower_components/perfect-scrollbar/js/min/perfect-scrollbar.jquery.min.js",
            //     "~/bower_components/jquery.cookie/jquery.cookie.js",
            //     "~/bower_components/sweetalert/dist/sweetalert.min.js",
            //     "~/bower_components/jquery.transit/jquery.transit.js",
            //     "~/bower_components/jquery_appear/jquery.appear.js",
            //     "~/bower_components/flexslider/jquery.flexslider-min.js",
            //     "~/assets/js/min/index.min.js",
            //        "~/assets/plugins/slider-revolution/js/jquery.themepunch.revolution.min.js?rev=5.0",
            //        "~/assets/plugins/slider-revolution/js/source/jquery.themepunch.tools.min.js",
            //        "~/assets/plugins/slider-revolution/js/jquery.themepunch.tools.min.js?rev=5.0",
            //    "~/bower_components/jquery.stellar/jquery.stellar.min.js",
            //    "~/bower_components/bootbox.js/bootbox.js",
            //    "~/bower_components/jquery-mockjax/dist/jquery.mockjax.min.js",
            //    "~/bower_components/select2/dist/js/select2.min.js",
            //    "~/bower_components/datatables/media/js/jquery.dataTables.min.js",
            //    "~/bower_components/datatables/media/js/dataTables.bootstrap.js",               
            //    "~/assets/js/min/table-data.min.js",

            //    "~/bower_components/jquery-colorbox/jquery.colorbox-min.js",
            //    "~/bower_components/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
            //     "~/assets/js/min/main.min.js",
            //    "~/bower_components/blockUI/jquery.blockUI.js",

            //     "~/assets/js/min/index.min.js",
            //       "~/assets/plugins/slider-revolution/js/jquery.themepunch.revolution.min.js?rev=5.0",
            //       "~/assets/plugins/slider-revolution/js/source/jquery.themepunch.tools.min.js",
            //       "~/assets/plugins/slider-revolution/js/jquery.themepunch.tools.min.js?rev=5.0",

            //  "~/assets/js/min/main.min.js"));


           


            //bundles.Add(new StyleBundle("~/bundles/CSS").Include(
            //    "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
            //    "~/bower_components/font-awesome/css/font-awesome.min.css",               
            //    "~/bower_components/iCheck/skins/all.css",
            //    "~/bower_components/perfect-scrollbar/css/perfect-scrollbar.min.css",
            //    "~/bower_components/sweetalert/dist/sweetalert.css",
            //    "~/bower_components/select2/dist/css/select2.min.css", 
            //    "~/bower_components/animate.css/animate.min.css",
            //    "~/bower_components/flexslider/flexslider.css",
            //    "~/bower_components/jquery-colorbox/example2/colorbox.css",
            //    "~/Content/themes/base/tooltip.css",
            //    "~/bower_components/datatables/media/css/dataTables.bootstrap.min.css",

            //    "~/assets/css/main.min.css",
            //   "~/assets/css/main-responsive.min.css",
            //   "~/assets/css/print.min.css",
            //   "~/assets/css/theme_blue.min.css",
            //   "~/assets/fonts/clip-font.min.css",
            //   "~/assets/css/main-responsive.min.css",
            //   "~/assets/plugins/slider-revolution/css/settings.css",
            //    "~/assets/plugins/slider-revolution/css/layers.css",
            //    "~/assets/plugins/slider-revolution/css/navigation.css",
            //   "~/assets/css/theme/light.min.css"));

            //BundleTable.EnableOptimizations = true;
            //bundles.UseCdn = true;
        }
    }
}
