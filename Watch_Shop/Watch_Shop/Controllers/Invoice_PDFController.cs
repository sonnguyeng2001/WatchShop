using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{
    public class Invoice_PDFController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: Invoice_PDF
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Invoice_Customer(int order_ID)
        {
            var item = db.Orders.Where(i => i.Order_ID == order_ID).SingleOrDefault();
            ViewBag.orderList = db.OrderDetails.Where(x => x.Order_ID == order_ID).ToList();
            return View(item);
        }

        public ActionResult Invoice_Customer_SendView(int order_ID)
        {
            var item = db.Orders.Where(i => i.Order_ID == order_ID).SingleOrDefault();
            return View(item);
        }


        public ActionResult Invoice_Import(int import_ID)
        {
            var item = db.Imports.Where(i => i.Import_ID == import_ID).SingleOrDefault();
            return View(item);
        }
    }
}