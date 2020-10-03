using System.Web.Optimization;

namespace SignalRMVCChat
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

    
            #region admin them
            //bundles.Add(new StyleBundle("~/bundles/css/customer").Include(
            //    "~/Content/adminty-master/files/assets/css/style.css",
            //    "~/Content/adminty-master/files/bower_components/bootstrap/css/bootstrap.min.css",
            //    "~/Content/adminty-master/files/assets/css/bootstrap_rtl.css",
            //    "~/Content/adminty-master/files/assets/icon/themify-icons/themify-icons.css",
            //    "~/Content/adminty-master/files/assets/icon/icofont/css/icofont.css",
            //    "~/Content/adminty-master/files/assets/icon/feather/css/feather.css",
            //    "~/Content/adminty-master/files/assets/pages/prism/prism.css",
            //    "~/Content/adminty-master/files/assets/css/jquery.mCustomScrollbar.css"
            //));
            
            //bundles.Add(new ScriptBundle("~/bundles/js/customer").Include(
            //    "~/Content/adminty-master/files/bower_components/jquery/js/jquery.min.js",
            //    "~/Content/adminty-master/files/bower_components/jquery-ui/js/jquery-ui.min.js",
            //    "~/Content/adminty-master/files/bower_components/popper.js/js/popper.min.js",
            //    "~/Content/adminty-master/files/bower_components/bootstrap/js/bootstrap.min.js",
            //    "~/Content/adminty-master/files/bower_components/jquery-slimscroll/js/jquery.slimscroll.js",
            //    "~/Content/adminty-master/files/bower_components/modernizr/js/modernizr.js",
            //    "~/Content/adminty-master/files/bower_components/modernizr/js/modernizr.js",
            //    "~/Content/adminty-master/files/bower_components/modernizr/js/css-scrollbars.js",
            //    "~/Content/adminty-master/files/assets/pages/prism/custom-prism.js",
            //    "~/Content/adminty-master/files/bower_components/i18next/js/i18next.min.js",
            //    "~/Content/adminty-master/files/bower_components/i18next-xhr-backend/js/i18nextXHRBackend.min.js",
            //    "~/Content/adminty-master/files/bower_components/i18next-browser-languagedetector/js/i18nextBrowserLanguageDetector.min.js",
            //    "~/Content/adminty-master/files/bower_components/jquery-i18next/js/jquery-i18next.min.js",
            //    "~/Content/adminty-master/files/assets/js/pcoded.min.js",
            //    "~/Content/adminty-master/files/assets/js/menu/menu-rtl.js",
            //    "~/Content/adminty-master/files/assets/js/jquery.mCustomScrollbar.concat.min.js",
            //    "~/Content/adminty-master/files/assets/js/script.js"
            //));
          
          
            

            #endregion


            #region flikr theme
            bundles.Add(new ScriptBundle("~/bundles/js/flikr").Include(
                "~/Content/ThemeFlikr/lifetrakr/plugins/jquery/jquery.js",
                "~/Content/ThemeFlikr/lifetrakr/plugins/bootstrap/bootstrap.min.js",
                "~/Content/ThemeFlikr/lifetrakr/plugins/slick/slick.min.js",
                "~/Content/ThemeFlikr/lifetrakr/js/custom.js"
                ));
          
            //bundles.Add(new StyleBundle("~/bundles/css/flikr").Include(
            //    "~/Content/ThemeFlikr/lifetrakr/plugins/bootstrap/bootstrap.min.css",
            //    "~/Content/ThemeFlikr/lifetrakr/plugins/themify-icons/themify-icons.css",
            //    "~/Content/ThemeFlikr/lifetrakr/plugins/slick/slick.css",
            //    "~/Content/ThemeFlikr/lifetrakr/plugins/slick/slick-theme.css",
            //    "~/Content/ThemeFlikr/lifetrakr/css/style.css"
            //));
            

            #endregion
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
        }
    }
}
