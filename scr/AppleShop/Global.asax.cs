using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AppleShop.Models;

namespace AppleShop
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // CSDL được tạo bằng script AppleShopDB_Script.sql — EF không tự tạo/migrate
            Database.SetInitializer<ApplicationDbContext>(null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Tạo sẵn vai trò Admin/Customer và 2 tài khoản mặc định nếu chưa có
            IdentitySeeder.Seed();
        }
    }
}
