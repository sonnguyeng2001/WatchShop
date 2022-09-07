using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{
    public class Order_HistoryController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: Order_History
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderHistory(int user_ID)
        {
            var item = db.Orders.Where(x => x.UserID == user_ID).OrderByDescending(x=>x.Create_Date).ToList();
            return View(item);
        }
        public JsonResult GetOrderList(int order_ID)
        {
            try
            {
                var item = db.OrderDetails.Where(x => x.Order_ID == order_ID).Select(x => new
                {
                    x.OrderID_STT,
                    x.Product_Name,
                    x.Product_Quantity,
                    x.Product.Price,
                    x.Status,
                    x.Into_Money
                }).ToList();
                return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}