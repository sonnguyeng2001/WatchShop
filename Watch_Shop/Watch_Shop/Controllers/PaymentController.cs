using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{
    public class PaymentController : Controller
    {
        static decimal Discount_Price = 0;
        static int Discount_ID = 0;
        static int Coins = 0;
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: Payment

        public ActionResult Payment()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            Session["Total"] = TongThanhTien();
            if (lstGioHang == null)
            {
                return RedirectToAction("ShowProduct", "Product");
            }
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult Payment(FormCollection f)
        {

            try
            {
                List<GioHang> lstGioHang = (List<GioHang>)Session["GioHang"];
                if (Session["GioHang"] == null || lstGioHang.Count == 0)
                {
                    return RedirectToAction("GioHang", "Cart");
                }
                if (ModelState.IsValid)
                {

                    decimal Total = Convert.ToDecimal(TongThanhTien());
                    if (Session["User_ID"] == null)
                    {
                        Order order = new Order();
                        order.Name = f["Name"];
                        order.Email = f["Email"];
                        order.Phone = f["Phone"];
                        order.Address = f["Address"];
                        order.City = f["City"];
                        order.Note = f["Note"];
                        order.Create_Date = DateTime.Now;
                        order.Total = Total - Discount_Price;
                        order.Status = "Verify...".ToString();
                        order.Discount = Discount_Price;
                        db.Orders.Add(order);
                        db.SaveChanges();
                        int OrderDetails_ID = order.Order_ID;
                        foreach (var item in lstGioHang)
                        {
                            OrderDetail od = new OrderDetail();
                            od.Order_ID = OrderDetails_ID;
                            od.Product_ID = item.iProduct_ID;
                            od.Product_Name = item.sProduct_Name;
                            od.Product_Quantity = item.iProduct_SoLuong;
                            od.Into_Money = Convert.ToDecimal(item.dProduct_ThanhTien);
                            var itemProduct = db.Products.Where(i => i.Product_ID == item.iProduct_ID).SingleOrDefault();
                            if (itemProduct != null)
                            {
                                itemProduct.Sales += item.iProduct_SoLuong;

                                if (itemProduct.Quantity - item.iProduct_SoLuong < 0)
                                {
                                    //---- Sản phẩm này không thể mua do số lượng đã hết
                                    od.Status = false;
                                }
                                else
                                {
                                    itemProduct.Quantity -= item.iProduct_SoLuong;
                                    od.Status = true;

                                }

                            }
                            db.OrderDetails.Add(od);
                        }
                        db.SaveChanges();
                        //SendMailll(f["Name"], f["Phone"], f["Email"], f["Address"], Total);
                    }
                    else
                    {
                        int userID = Convert.ToInt32(Session["User_ID"]);
                        User us = db.Users.SingleOrDefault(u => u.UserID == userID);

                        Order order = new Order();
                        order.UserID = userID;
                        order.Name = us.CustomerName;
                        order.Email = us.CustomerEmail;
                        order.Phone = us.CustomerPhone;
                        order.Address = us.CustomerAddress;
                        order.Create_Date = DateTime.Now;
                        order.City = us.CustomerCity;
                        order.Note = f["Note"];
                        order.Total = Total - Discount_Price - (Coins * 10000);
                        order.Status = "Verify...".ToString();
                        order.Discount = Discount_Price;
                        order.Point = Coins * 10000;


                        // Tích điểm: 1.000.000 = 1 xu
                        // Đổi điểm: 1xu = 10.000

                        int point = (int)(order.Total / 1000000);
                        if (us.RewardPoints == null || us.RewardPoints <= 0)
                        {
                            us.RewardPoints = 0;
                        }

                        // Khách hàng không áp dụng xu khuyến mãi
                        if (Coins == 0)
                        {
                            us.RewardPoints += point;
                        }
                        else // Khách hàng áp dụng xu khuyến mãi
                        {
                            us.RewardPoints = 0;
                        }

                        db.Orders.Add(order);
                        db.SaveChanges();

                        int OrderDetails_ID = order.Order_ID;
                        foreach (var item in lstGioHang)
                        {
                            OrderDetail od = new OrderDetail();
                            od.Order_ID = OrderDetails_ID;
                            od.Product_ID = item.iProduct_ID;
                            od.Product_Name = item.sProduct_Name;
                            od.Product_Quantity = item.iProduct_SoLuong;
                            od.Into_Money = Convert.ToDecimal(item.dProduct_ThanhTien);
                            var itemProduct = db.Products.Where(i => i.Product_ID == item.iProduct_ID).SingleOrDefault();
                            if (itemProduct != null)
                            {
                                itemProduct.Sales += item.iProduct_SoLuong;

                                if (itemProduct.Quantity - item.iProduct_SoLuong < 0)
                                {
                                    //---- Sản phẩm này không thể mua do số lượng muốn mua > số lượng trong kho
                                    od.Status = false;
                                }
                                else
                                {
                                    itemProduct.Quantity -= item.iProduct_SoLuong;
                                    od.Status = true;

                                }

                            }
                            db.OrderDetails.Add(od);
                        }

                        db.SaveChanges();
                        //SendView(OrderDetails_ID, us.CustomerEmail, "Đơn hàng mới từ Watch Shop");
                    }
                    if (Discount_ID != 0)
                    {
                        var item = db.Discounts.SingleOrDefault(x => x.Discount_ID.Equals(Discount_ID));
                        if (item != null)
                        {
                            item.Discount_Quantity -= 1;
                            db.SaveChanges();
                        }
                    }
                    //--------------------------- Gửi email cho khách ------------------------------
                }
                Discount_Price = 0;
                Discount_ID = 0;
                Coins = 0;
                Session["GioHang"] = null;
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });

            }
        }
        public void SendMailll(string Name, string Phone, string Email, string Address, decimal Total)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/neworder.html"));
            content = content.Replace("{{CustomerName}}", Name);
            content = content.Replace("{{Phone}}", Phone);
            content = content.Replace("{{Email}}", Email);
            content = content.Replace("{{Address}}", Address.ToString());
            content = content.Replace("{{Total}}", string.Format("{0:#,##0.}", Total));

            // Gửi mail cho khách hàng
            new MailHelper().SendMail(Email, "Đơn hàng mới từ WatchShop - " + Name + "", content);

            // Gửi mail cho Admin
            var Email_Admin = System.Configuration.ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new MailHelper().SendMail(Email_Admin, "Đơn hàng mới từ WatchShop - Admin", content);
        }
        // Tổng thành tiền
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
        public JsonResult Discount(string value, int status)
        {
            var item = db.Discounts.FirstOrDefault(x => x.Discount_Name.Equals(value.Trim().ToUpper()));

            if (status == 1)
            {
                if (item != null)
                {
                    if (DateTime.Now > item.Expiration_Date)
                    {
                        return Json(new { status = false, msg = "Voucher đã hết hạn sử dụng !!!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (item.Discount_Quantity <= 0)
                    {
                        return Json(new { status = false, msg = "Số lượng vouchet đã hết !!!" }, JsonRequestBehavior.AllowGet);

                    }
                    Discount_Price = Convert.ToDecimal(item.Discount_Price);
                    Discount_ID = item.Discount_ID;
                    return Json(new { status = true, msg = "Áp dụng voucher thành công ", price = item.Discount_Price }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = false, msg = "Mã voucher không tồn tại !!!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //status = -1

                Discount_Price = 0;
                Discount_ID = 0;
                return Json(new { status = true, price = item.Discount_Price }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult functionCoins(int status)
        {
            int userID = Convert.ToInt32(Session["User_ID"]);
            var item = db.Users.Where(x => x.UserID == userID).SingleOrDefault();
            if(status == 1)
            {
                if (item != null)
                {
                    Coins = (int)item.RewardPoints;
                    return Json(new { status = true, Coins = item.RewardPoints }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = false });
            }
            else
            {
                // status = -1 
                Coins = 0;
                return Json(new { status = true, Coins = item.RewardPoints }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Value_Discount_Coins()
        {
            Discount_Price = 0;
            Discount_ID = 0;
            Coins = 0;
            return Json(new { status = true });
        }

        //TODO: Render Razor View To String

        [HttpGet]
        public ActionResult Index()
        {
            var item = db.OrderDetails.ToList();
            return View(item);
        }

        public ActionResult SendView(int oder_ID, string email, string title)
        {
            var item = db.Orders.Where(x => x.Order_ID == oder_ID).SingleOrDefault();
            String renderHTML = new ViewToStringController().RenderViewToString("Invoice_PDF", "Invoice_Customer_SendView", item);
            new MailHelper().SendMail(email, title, renderHTML);
            return View(item);

        }


        //TODO:Get coins Customer
        public JsonResult GetCoins()
        {
            if (Session["User_ID"] == null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            int userID = Convert.ToInt32(Session["User_ID"]);
            var item = db.Users.Where(x => x.UserID == userID).SingleOrDefault();
            if (item != null)
            {
                return Json(new { status = true, CoinsValue = item.RewardPoints }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false });
        }
    }
}