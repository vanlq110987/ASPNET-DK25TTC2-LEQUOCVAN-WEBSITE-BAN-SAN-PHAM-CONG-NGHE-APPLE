using System.Web.Mvc;
using AppleShop.Models;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>Lớp cơ sở cho toàn bộ controller khu vực quản trị — chỉ Admin truy cập.</summary>
    [Authorize(Roles = "Admin")]
    public abstract class AdminControllerBase : Controller
    {
        protected readonly ApplicationDbContext db = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
