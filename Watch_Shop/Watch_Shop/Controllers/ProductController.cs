using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Threading;


namespace Watch_Shop.Controllers
{
    public class ProductController : Controller
    {
        // Home Page
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowProduct() // Danh sách sản phẩm
        {
            var item = db.Products.ToList();
            ViewBag.Watch_Male = db.Products.Where(sp => sp.Category_ID == 1).Take(8).ToList();
            ViewBag.Watch_Femal = db.Products.Where(sp => sp.Category_ID == 2).Take(8).ToList();
            ViewBag.Watch_Couple = db.Products.Where(sp => sp.Category_ID == 3).ToList();
            return View(item);
        }
        public JsonResult LoadMoreProduct(int CateID, int numberClick)
        {
            string check = "";
            int session_id = 0;
            if (Session["Username"] != null && Session["Username"].ToString() != "admin")
            {
                check = "Customer";
                session_id = int.Parse(Session["User_ID"].ToString());
            }
            try
            {
                var listTotal = db.Products.Where(x => x.Category_ID == CateID).ToList();
                List<Product> products = new List<Product>();
                for (int i = 0; i < listTotal.Count; i++)
                {
                    if (i >= 4 * (numberClick - 1) && i < 4 * numberClick)
                    {
                        products.Add(listTotal[i]);
                    }
                }
                var chooseProduct = products.Select(x => new
                {
                    x.Product_ID,
                    x.Product_Name,
                    x.Price,
                    x.Image,
                    x.Quantity
                }).ToList();
                return Json(new { status = true, data = chooseProduct, quantity = chooseProduct.Count, Check = check, Customer_ID = session_id }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public List<Product> ShowProduct_Top() // Danh sách sản phẩm bán chạy
        {
            var item = db.Products.Take(5).OrderByDescending(x => x.Sales).ToList();
            return item;
        }
        public List<Product> List_Related_Product(int Pro_ID) // Danh sách sản phẩm liên quan
        {
            var product = db.Products.Find(Pro_ID);
            return db.Products.Where(sp => sp.Product_ID != Pro_ID && sp.Category_ID == product.Category_ID).ToList();
        }
        public ActionResult ShowProduct_Id(int id) //  Chi tiết sản phẩm
        {
            var item = db.Products.Include("Category").SingleOrDefault(p => p.Product_ID == id);
            if (item == null)
            {
                return RedirectToAction("Index", "page404_");
            }
            ViewBag.Related_Product = List_Related_Product(id);
            ViewBag.Product_Top5 = ShowProduct_Top();
            return View(item);
        }
        public ActionResult Show_PhuKien()
        {
            var item = db.Categories.ToList();
            return PartialView(item);
        }
        public JsonResult Find_Product(string keyword)
        {
            try
            {
                var item = db.Products.Where(x => x.Product_Name.Contains(keyword)).Select(x => new
                {
                    x.Product_ID,
                    x.Product_Name,
                    x.Image,
                    x.Price
                }).ToList();
                return Json(new { status = true, count = item.Count(), data = item }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false, msg = "Có lỗi trong quá trình xử lý !!!" }, JsonRequestBehavior.AllowGet);

            }

        }


    }
}