using System.Web;
using System.Web.Optimization;

namespace Autocor.Catalogo.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/plugins/jquery/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/plugins/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/plugins/bootstrap/js/bootstrap.js",
                      "~/Assets/plugins/respond/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Assets/plugins/bootstrap/css/bootstrap.css",                      
                      "~/Assets/font-awesome/css/font-awesome.css",
                      "~/Assets/css/estilosHome.css",
                      "~/Assets/css/estilos.css",
                      "~/Assets/plugins/overhang/overhang.min.css",
                      "~/Assets/css/estilosCatalogo.css", 
                      "~/Assets/plugins/fontello/css/fontello.css",
                      "~/Assets/plugins/fontello/css/animation.css"));
        }
    }
}
