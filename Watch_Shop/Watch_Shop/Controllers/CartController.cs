using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{
    public class CartController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                ViewBag.Product = "Your Cart is null";
                return View(lstGioHang);
            }
            ViewBag.TongSoLuongg = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            if (lstGioHang == null)
            {
                return RedirectToAction("ShowProduct", "Product");
            }
            return View(lstGioHang);
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = (List<GioHang>)Session["GioHang"];
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult GioHangPartial()
        {
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView(lstGioHang);
        }
        public ActionResult Add_Product(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang SanPham = lstGioHang.Find(sp => sp.iProduct_ID == id);

            if (SanPham == null)
            {
                SanPham = new GioHang(id);
                lstGioHang.Add(SanPham);
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                SanPham.iProduct_SoLuong++;
                return Json(new
                {
                    status = true
                });
            }
        }
        [HttpPost]
        public JsonResult Add_Product_Quantity(int Product_id, int Quantity)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang SanPham = lstGioHang.Find(sp => sp.iProduct_ID == Product_id);

            if (SanPham == null)
            {
                SanPham = new GioHang(Product_id);
                lstGioHang.Add(SanPham);
                SanPham.iProduct_SoLuong += Quantity - 1;
                return Json(new { status = true });
            }
            else
            {
                SanPham.iProduct_SoLuong += Quantity;
                return Json(new { status = true });

            }

        }
        [HttpPost]
        public ActionResult Delete_Product(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.RemoveAll(sp => sp.iProduct_ID == id);
            return Json(new
            {
                status = true
            });
        }
        private double TongThanhTien()
        {
            double ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.dProduct_ThanhTien);
            }
            return ttt;
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl += lstGioHang.Sum(sp => sp.iProduct_SoLuong);
            }
            return tsl;
        }
        public ActionResult Update(string cartModel)
        {
            try
            {

                List<GioHang> Jsoncart = new JavaScriptSerializer().Deserialize<List<GioHang>>(cartModel);
                var SessionCart = (List<GioHang>)Session["GioHang"];

                foreach (var item in SessionCart)
                {
                    var jsonItem = Jsoncart.SingleOrDefault(x => x.iProduct_ID == item.iProduct_ID);
                    if (jsonItem != null)
                    {
                        item.iProduct_SoLuong = jsonItem.iProduct_SoLuong;
                        var db_Product = db.Products.Where(i => i.Product_ID == item.iProduct_ID).SingleOrDefault();
                        if (db_Product.Quantity < jsonItem.iProduct_SoLuong)
                        {
                            item.iProduct_SoLuong = (int)db_Product.Quantity;
                        }
                    }
                }
                Session["GioHang"] = SessionCart;
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public ActionResult DeleteAll()
        {
            List<GioHang> lst = LayGioHang();
            lst.Clear();
            return Json(new
            {
                status = true
            });
        }

        public JsonResult CheckQuantity(int Product_id, int Quantity)
        {
            try
            {
                var item = db.Products.Where(x => x.Product_ID == Product_id).SingleOrDefault();
                var cartList = (List<GioHang>)Session["GioHang"];
                var cartItem = cartList.Where(i=>i.iProduct_ID == Product_id).FirstOrDefault();

                if (item.Quantity - Quantity < 0)
                {
                    cartItem.iProduct_SoLuong = (int)item.Quantity;
                    return Json(new { status = true, Invalid = false, limitedQuantity = item.Quantity });
                }
                else
                {
                    cartItem.iProduct_SoLuong = Quantity;
                    return Json(new { status = true, Invalid = true });
                }
            }
            catch
            {
                return Json(new { status = false });
            }

        }
    }
}