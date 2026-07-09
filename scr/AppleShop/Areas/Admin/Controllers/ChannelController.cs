using System.Linq;
using System.Web.Mvc;
using AppleShop.Helpers;
using AppleShop.Models;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>CRUD Kênh phân phối — kiểm tra ràng buộc tồn kho trước khi xóa.</summary>
    public class ChannelController : AdminControllerBase
    {
        // GET: /Admin/Channel
        public ActionResult Index()
        {
            var channels = db.DistributionChannels
                .OrderBy(c => c.Name)
                .ToList();
            ViewBag.InventoryCounts = db.Inventories
                .GroupBy(i => i.ChannelId)
                .ToDictionary(g => g.Key, g => g.Count());
            return View(channels);
        }

        // GET: /Admin/Channel/Create
        public ActionResult Create()
        {
            return View(new DistributionChannel());
        }

        // POST: /Admin/Channel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DistributionChannel model)
        {
            model.Slug = SlugHelper.ToSlug(model.Name);
            ModelState.Remove("Slug");
            if (!ModelState.IsValid) return View(model);

            if (db.DistributionChannels.Any(c => c.Slug == model.Slug))
            {
                ModelState.AddModelError("Name", "Kênh phân phối này đã tồn tại.");
                return View(model);
            }

            db.DistributionChannels.Add(model);
            db.SaveChanges();

            TempData["Success"] = "Đã thêm kênh phân phối \"" + model.Name + "\".";
            return RedirectToAction("Index");
        }

        // GET: /Admin/Channel/Edit/5
        public ActionResult Edit(int id)
        {
            var channel = db.DistributionChannels.Find(id);
            if (channel == null) return HttpNotFound();
            return View(channel);
        }

        // POST: /Admin/Channel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DistributionChannel model)
        {
            var channel = db.DistributionChannels.Find(model.ChannelId);
            if (channel == null) return HttpNotFound();

            ModelState.Remove("Slug");
            if (!ModelState.IsValid) return View(model);

            channel.Name = model.Name;
            channel.Slug = SlugHelper.ToSlug(model.Name);
            channel.Website = model.Website;
            channel.Hotline = model.Hotline;
            channel.Address = model.Address;
            channel.IsActive = model.IsActive;
            db.SaveChanges();

            TempData["Success"] = "Đã cập nhật kênh phân phối \"" + channel.Name + "\".";
            return RedirectToAction("Index");
        }

        // POST: /Admin/Channel/Delete/5 — chặn xóa nếu còn tồn kho tham chiếu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var channel = db.DistributionChannels.Find(id);
            if (channel == null) return HttpNotFound();

            if (db.Inventories.Any(i => i.ChannelId == id))
            {
                TempData["Error"] = "Không thể xóa \"" + channel.Name + "\": đang có tồn kho tham chiếu đến kênh này.";
                return RedirectToAction("Index");
            }

            db.DistributionChannels.Remove(channel);
            db.SaveChanges();

            TempData["Success"] = "Đã xóa kênh phân phối \"" + channel.Name + "\".";
            return RedirectToAction("Index");
        }
    }
}
