using System.Web.Mvc;
using System.Web.Routing;

namespace AppleShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // URL thân thiện: /san-pham/{slug}
            routes.MapRoute(
                name: "ProductDetail",
                url: "san-pham/{slug}",
                defaults: new { controller = "Product", action = "Detail" });

            // URL thân thiện: /tin-tuc/{slug}
            routes.MapRoute(
                name: "NewsDetail",
                url: "tin-tuc/{slug}",
                defaults: new { controller = "News", action = "Detail" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AppleShop.Controllers" });
        }
    }
}
