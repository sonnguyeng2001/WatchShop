using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{

    public class UserController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: User
        public ActionResult Login()
        {
            ViewBag.Error = 0;
            return PartialView();
        }
        [HttpPost]
        public ActionResult CheckLogin(string username, string password)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) // Kiểm tra rỗng
                {
                    return Json(new { status = false, msg = "Vui  lòng điền đầy đủ thông tin. !!!" });
                }
                else // Nếu như khác rỗng
                {
                    User user = db.Users.SingleOrDefault(c => c.Username == username && c.Password == password);
                    if (user != null) //  Đăng nhập thành công
                    {
                        Session["User_ID"] = user.UserID;
                        Session["Username"] = user.Username;
                        if (user.Permission == 1)//Nếu là admin
                        {
                            return Json(new { status = true, role = "admin" });
                        }
                        else //Nếu là user
                        {
                            return Json(new { status = true, role = "user", user_id = user.UserID });
                        }
                    }
                    else
                    {
                        // Sai tên đăng nhập hoặc mật khẩu
                        return Json(new { status = false, msg = "Tên đăng nhập hoặc mật khẩu không chính xác !!!" });
                    }
                }
            }
            return Json(new { status = false ,msg = "Lỗi không xác định" }); // Các trường hợp không hợp lệ còn lại

        }
        public ActionResult Customer_Information(int Customer_ID)
        {
            var item = db.Users.Where(c => c.UserID == Customer_ID).SingleOrDefault();
            if (item != null)
            {
                return View(item);
            }
            return HttpNotFound();
        }
        public JsonResult Update_Info_Customer(User updateUser)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.SingleOrDefault(us => us.UserID == updateUser.UserID);
                if (user != null)
                {
                    user.Username = updateUser.Username;
                    user.Password = updateUser.Password;
                    user.CustomerName = updateUser.CustomerName;
                    user.CustomerEmail = updateUser.CustomerEmail;
                    user.CustomerPhone = updateUser.CustomerPhone;
                    user.CustomerAddress = updateUser.CustomerAddress;
                    user.CustomerCity = updateUser.CustomerCity;
                    db.SaveChanges();
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false,msg = "Có lỗi trong quá trình cập nhật" }) ;

                }
            }
            return Json(new { status = false, msg = "Có lỗi trong quá trình cập nhật" });

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("ShowProduct", "Product");

        }
        public ActionResult ForgetPasword()
        {
            return View();
        }
        public string CreateToken()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 8)
               .Select(token => token[random.Next(token.Length)]).ToArray());

            return resultToken.ToString();
        }
        public JsonResult SendToken(string ToEmail)
        {
            try
            {
                string token = CreateToken();
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/sendToken.html"));
                content = content.Replace("{{Token}}", token);
                var item = db.Users.Where(x => x.CustomerEmail == ToEmail.Trim()).FirstOrDefault();
                if (item != null)
                {
                    item.CustomerToken = token;
                    db.SaveChanges();
                    new MailHelper().SendMail(ToEmail, "Cấp lại mật khẩu", content);
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            catch
            {
                return Json(new { status = false });

            }
        }
        public JsonResult ConfirmToken(string ToEmail, string token)
        {
            try
            {
                var item = db.Users.Where(x => x.CustomerEmail == ToEmail.Trim() && x.CustomerToken == token.ToUpper().Trim()).FirstOrDefault();
                if (item != null)
                {
                    return Json(new { status = true, data = item.Password });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        public JsonResult Register(User newUser)
        {
            try
            {
                if (String.IsNullOrEmpty(newUser.CustomerEmail) || String.IsNullOrEmpty(newUser.CustomerPhone) || String.IsNullOrEmpty(newUser.Username) || String.IsNullOrEmpty(newUser.Password) || String.IsNullOrEmpty(newUser.CustomerCity) || String.IsNullOrEmpty(newUser.CustomerAddress))
                {
                    return Json(new { status = false, msg = "Vui lòng điền đầy đủ thông tin !!!" });

                }
                var list = db.Users.ToList();
                foreach (var item in list)
                {
                    if (String.IsNullOrEmpty(item.CustomerEmail) || String.IsNullOrEmpty(item.CustomerPhone) || String.IsNullOrEmpty(item.Username))
                    {
                        continue;
                    }
                    if (newUser.CustomerEmail.Trim() == item.CustomerEmail)
                    {
                        return Json(new { status = false, msg = "Email này đã được đăng ký bởi 1 tài khoản khác !!!" });

                    }
                    if (newUser.CustomerPhone.Trim() == item.CustomerPhone.Trim())
                    {
                        return Json(new { status = false, msg = "Số điện thoại này đã được đăng ký bởi 1 tài khoản khác !!!" });

                    }
                    if (newUser.Username.Trim() == item.Username.Trim())
                    {
                        return Json(new { status = false, msg = "Tên tài khoản đã tồn tại !!!" });

                    }
                }
                newUser.Permission = 0;
                db.Users.Add(newUser);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        public JsonResult Favorite_Product(int Customer_ID, int Product_ID)
        {
            var item = db.Customer_Product.Where(x => x.Customer_ID == Customer_ID && x.Product_ID == Product_ID).SingleOrDefault();
            if(item == null)
            {
                Customer_Product cs = new Customer_Product();
                cs.Customer_ID = Customer_ID;
                cs.Product_ID = Product_ID;
                db.Customer_Product.Add(cs);
                db.SaveChanges();
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }
        public JsonResult Delete_Favorite_Product(int Customer_ID, int Product_ID)
        {
            var item = db.Customer_Product.Where(x => x.Customer_ID == Customer_ID && x.Product_ID == Product_ID).SingleOrDefault();
            if (item != null)
            {
                db.Customer_Product.Remove(item);
                db.SaveChanges();
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }
        public ActionResult Page_Product_Favorite(int Customer_ID)
        {
            var item = db.Customer_Product.Where(x => x.Customer_ID == Customer_ID).ToList();
            ViewBag.Quantity = item.Count;
            return View(item);
        }
        public ActionResult Page_Product_Favorite_Partial(int user_ID)
        {
            ViewBag.Quantity = db.Customer_Product.Where(x => x.Customer_ID == user_ID).Count();
            return PartialView();
        }
    }
}