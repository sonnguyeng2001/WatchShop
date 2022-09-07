using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{
    public class FeedBackController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: FeedBack
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FeedBack_Product(int Product_id)
        {
            var item = db.FeedBacks.Where(x => x.Product_ID == Product_id).OrderByDescending(x => x.FeedBack_ID).Select(x => new
            {
                x.FeedBack_ID,
                x.FeedBack_User,
                x.FeedBack_Content,
                x.FeedBack_Time,
            }).ToList();
            if (item.Count > 0)
            {
                return Json(new { status = true, data = item, quantity = item.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddFeedBack(FeedBack fb)
        {
            if (String.IsNullOrEmpty(fb.FeedBack_Content) || String.IsNullOrEmpty(fb.FeedBack_User))
            {
                return Json(new { status = false });
            }
            db.FeedBacks.Add(fb);
            db.SaveChanges();
            return Json(new { status = true });
        }

        public JsonResult deleteFeedback(int id)
        {
            var item = db.FeedBacks.Where(x => x.FeedBack_ID == id).SingleOrDefault();
            if (item != null)
            {
                db.FeedBacks.Remove(item);
                db.SaveChanges();
                return Json(new { status = true });
            }
            return Json(new { status = false });

        }
    }
}