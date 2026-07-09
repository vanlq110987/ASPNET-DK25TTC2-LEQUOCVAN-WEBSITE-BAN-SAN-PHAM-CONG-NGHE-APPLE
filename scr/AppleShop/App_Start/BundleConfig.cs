using System.Web.Optimization;

namespace AppleShop
{
    public class BundleConfig
    {
        // Bootstrap / jQuery / SB Admin 2 được nạp qua CDN trong Layout;
        // bundle chỉ gom CSS/JS tự viết của dự án.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/site.js"));
        }
    }
}
