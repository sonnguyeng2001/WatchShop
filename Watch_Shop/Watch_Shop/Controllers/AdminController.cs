using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{
    public class AdminController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: Admin

        #region Dashboard
        public ActionResult Dashboard()
        {
            ViewBag.MoneyMonth = db.Orders.Where(x => x.Create_Date.Value.Month == DateTime.Now.Month && x.Status == "Received").Sum(x => x.Total);
            ViewBag.MoneyYear = db.Orders.Where(x => x.Create_Date.Value.Year == DateTime.Now.Year && x.Status == "Received").Sum(x => x.Total);
            ViewBag.OrderMonth = db.Orders.Where(x => x.Create_Date.Value.Month == DateTime.Now.Month && x.Status == "Received").Count();
            return View();
        }

        public JsonResult Total_Month(int year)
        {
            try
            {
                List<int> myList = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    var item = db.Orders.Where(x => x.Create_Date.Value.Month == i && x.Create_Date.Value.Year == year && x.Status.ToString() == "Received").Sum(x => x.Total);
                    if (item == null)
                    {
                        item = 0;
                    }
                    myList.Add((int)item);
                }
                return Json(new { status = true, data = myList }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        #endregion

        #region Category
        // Category
        public ActionResult GetCate()
        {
            var list = db.Categories.ToList();
            return View(list);
        }
        [HttpPost]
        public JsonResult CreateCate(Category cate)
        {
            db.Categories.Add(cate);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
        [HttpPost]
        public JsonResult DeleteCate(int id)
        {
            try
            {
                var cate = db.Categories.Where(x => x.Category_ID == id).SingleOrDefault();
                db.Categories.Remove(cate);
                db.SaveChanges();
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
        public JsonResult DetailsCate(int id)
        {
            var result = db.Categories.SingleOrDefault(x => x.Category_ID.Equals(id));
            return Json(new { result.Category_ID, result.Category_Name }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateCate(Category cate)
        {
            Category old = db.Categories.SingleOrDefault(x => x.Category_ID.Equals(cate.Category_ID));
            if (old != null)
            {
                old.Category_ID = cate.Category_ID;
                old.Category_Name = cate.Category_Name;
            }
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
        public ActionResult Product_ByCate(int id)
        {
            var item = db.Products.Include("Category").Where(x => x.Category_ID == id).ToList();
            if (item.Count == 0)
            {
                return RedirectToAction("Index", "Page404_");
            }
            ViewBag.Name = db.Categories.Find(id).Category_Name.ToString();
            return View(item);
        }
        #endregion

        #region Product
        // Product
        public ActionResult GetPro()
        {
            List<Category> listCate = db.Categories.ToList();
            ViewBag.listCate = new SelectList(listCate, "Category_ID", "Category_Name");
            ViewBag.CateID = (from obj in db.Categories
                              select new SelectListItem()
                              {
                                  Text = obj.Category_Name,
                                  Value = obj.Category_ID.ToString()
                              }).ToList();
            return View();

        }
        public JsonResult GetProList()
        {
            var item = db.Products.Select(s => new
            {
                s.Product_ID,
                s.Product_Name,
                s.Price,
                s.Image,
                s.Quantity
            }).ToList();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProList_ByCateID(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var item = db.Products.Where(x => x.Category_ID == id).ToList();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DetailsPro(int id)
        {
            var item = db.Products.SingleOrDefault(x => x.Product_ID == id);

            return Json(new
            {
                item.Product_ID,
                item.Product_Name,
                item.Price,
                item.Description,
                item.Image,
                item.Aparatus_and_Energy,
                item.Material_Day,
                item.Material_Matkinh,
                item.Dial_HinhDang,
                item.Dial_KichThuoc,
                item.Dial_MauSac,
                item.Waterproof,
                item.TradeMark,
                item.Origin,
                item.Category_ID,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveImage_Folder(ImageViewModel img)
        {
            var filename = img.ImageFile;
            filename.SaveAs(Server.MapPath("~/image/") + Path.GetFileName(filename.FileName));
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreatePro(Product newPro)
        {
            db.Products.Add(newPro);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }); ;
        }
        public JsonResult DeletePro(int id)
        {
            var item = db.Products.SingleOrDefault(x => x.Product_ID.Equals(id));
            db.Products.Remove(item);
            db.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
        public JsonResult UpdatePro(Product newProduct)
        {
            var item = db.Products.SingleOrDefault(x => x.Product_ID.Equals(newProduct.Product_ID));
            if (item != null)
            {
                item.Product_Name = newProduct.Product_Name;
                item.Price = newProduct.Price;
                item.Description = newProduct.Description;
                item.Aparatus_and_Energy = newProduct.Aparatus_and_Energy;
                item.Material_Day = newProduct.Material_Day;
                item.Material_Matkinh = newProduct.Material_Matkinh;
                item.Dial_HinhDang = newProduct.Dial_HinhDang;
                item.Dial_KichThuoc = newProduct.Dial_KichThuoc;
                item.Dial_MauSac = newProduct.Dial_MauSac;
                item.Waterproof = newProduct.Waterproof;
                item.TradeMark = newProduct.TradeMark;
                item.Origin = newProduct.Origin;
                item.Category_ID = newProduct.Category_ID;
                if (String.IsNullOrEmpty(newProduct.Image))
                {
                    newProduct.Image = item.Image;
                }
                if (item.Image != newProduct.Image)
                {
                    item.Image = newProduct.Image;
                    db.SaveChanges();
                    return Json(new { status = true, newImage = true }, JsonRequestBehavior.AllowGet);
                }
                db.SaveChanges();
            }
            return Json(new { status = true, newImage = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchKey(string key)
        {
            var item = db.Products.Where(x => x.Product_Name.Contains(key)).Select(x => new
            {
                x.Product_ID,
                x.Product_Name,
                x.Price,
                x.Image,
                x.Quantity
            }).ToList();

            return Json(new { data = item, quantity = item.Count }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Order
        // Order
        public ActionResult GetOrder()
        {
            return View();
        }
        public JsonResult GetOrderList()
        {
            var item = db.Orders.Select(x => new
            {
                x.Order_ID,
                x.Create_Date,
                x.Total,
                x.Discount,
                x.Point,
                x.Status,
            }).ToList();
            return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteOrder(int id)
        {
            // ----------------- Xóa các Order Details --------------------
            var itemOrderDetails = db.OrderDetails.Where(i => i.Order_ID == id).ToList();

            if (itemOrderDetails.Count > 0)
            {
                foreach (var product in itemOrderDetails)
                {
                    var itemProduct = db.Products.Where(p => p.Product_ID == product.Product_ID).SingleOrDefault();
                    if (itemProduct != null)
                    {
                        itemProduct.Quantity += product.Product_Quantity;
                    }
                }
                db.SaveChanges();
            }

            // ----------------- Xóa Order -------------------------------

            var item = db.Orders.Where(x => x.Order_ID == id).SingleOrDefault();
            if (item != null)
            {
                try
                {
                    db.Orders.Remove(item);
                    db.SaveChanges();
                    return Json(new { status = true });
                }
                catch
                {
                    return Json(new { status = false });

                }
            }
            return Json(new { status = false });

        }
        public JsonResult DetailsOrder(int id)
        {
            var item = db.OrderDetails.Where(x => x.Order_ID == id).Select(x => new
            {
                x.OrderID_STT,
                x.Order_ID,
                x.Product_ID,
                x.Product_Name,
                x.Product_Quantity,
                x.Status,
                x.Into_Money,
            }).ToList();
            if (item != null)
            {
                return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DetailsOrderPage(int id)
        {
            var item = db.Orders.Where(x => x.Order_ID == id).SingleOrDefault();
            if(item != null)
            {
                return View(item);
            }
            return RedirectToAction("Index", "Page404_");
        }
        public JsonResult DeleteOrderDetails(int id)
        {
            var orderDetail = db.OrderDetails.Where(x => x.OrderID_STT == id).SingleOrDefault();
            var order = db.Orders.Where(z => z.Order_ID == orderDetail.Order_ID).SingleOrDefault();
            order.Total -= orderDetail.Into_Money;
            var product = db.Products.Where(p => p.Product_ID == orderDetail.Product_ID).SingleOrDefault();
            product.Quantity += orderDetail.Product_Quantity;
            try
            {
                db.OrderDetails.Remove(orderDetail);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });

            }
        }

        public JsonResult ChangeOrderStatus(int ID_Status, string Status)
        {
            try
            {
                var item = db.Orders.Where(x => x.Order_ID == ID_Status).SingleOrDefault();
                if (item != null)
                {
                    item.Status = Status;
                    db.SaveChanges();
                    return Json(new { status = true });
                }
                return Json(new { status = false });
            }
            catch
            {
                return Json(new { status = false });

            }
        }
        #endregion

        #region Discount
        //Discount
        public ActionResult GetDiscount()
        {
            return View();
        }
        public JsonResult ShowDiscountList()
        {
            var item = db.Discounts.OrderByDescending(x => x.Expiration_Date).ToList();
            return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDiscount(int id)
        {
            try
            {
                db.Discounts.Remove(db.Discounts.Where(x => x.Discount_ID == id).SingleOrDefault());
                db.SaveChanges();
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult AddDiscount(Discount newDis)
        {
            newDis.Discount_Name = newDis.Discount_Name.Trim().ToUpper();
            db.Discounts.Add(newDis);
            db.SaveChanges();
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DetailsDiscount(int id)
        {
            var item = db.Discounts.Where(x => x.Discount_ID == id).SingleOrDefault();
            if (item != null)
            {
                return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Acount
        //Account
        public ActionResult GetAccount()
        {
            var item = db.Users;
            return View(item.ToList());
        }
        public JsonResult ShowAccountList()
        {
            var item = db.Users.Select(x => new
            {
                x.UserID,
                x.Username,
                x.Password,
                x.Permission
            }).ToList();
            return Json(new
            {
                status = true,
                data = item,
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteAccount(int id)
        {
            try
            {
                var item = db.Users.Where(x => x.UserID == id).SingleOrDefault();
                db.Users.Remove(item);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });

            }
        }
        public JsonResult DetailsAccount(int id)
        {
            try
            {
                var item = db.Users.Where(x => x.UserID == id).Select(x => new
                {
                    x.Username,
                    x.Password,
                    x.Permission,
                    x.UserID,
                    x.CustomerName,
                    x.CustomerEmail,
                    x.CustomerPhone,
                    x.CustomerAddress,
                    x.CustomerCity
                }).SingleOrDefault();
                return Json(new { status = true, data = item });
            }
            catch
            {
                return Json(new { status = false });
            }

        }
        public JsonResult UpdateUsers(User newUser)
        {
            try
            {
                var user = db.Users.Where(x => x.UserID == newUser.UserID).SingleOrDefault();
                if (user != null)
                {
                    user.Username = newUser.Username;
                    user.Password = newUser.Password;
                    user.Permission = newUser.Permission;
                    user.CustomerName = newUser.CustomerName;
                    user.CustomerEmail = newUser.CustomerEmail;
                    user.CustomerPhone = newUser.CustomerPhone;
                    user.CustomerAddress = newUser.CustomerAddress;
                    user.CustomerCity = newUser.CustomerCity;
                    db.SaveChanges();
                }
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });

            }
        }
        #endregion

        #region Import
        public ActionResult Page_Import()
        {
            return View();
        }

        public JsonResult Get_Import_List()
        {
            var item = db.Imports.Select(x => new
            {
                x.Import_ID,
                x.Importer,
                x.Import_Date,
                x.Import_Total,
                x.Import_Note
            }).ToList();
            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Import_Details(int id)
        {
            try
            {
                var item = db.Import_Details.Where(x => x.Import_ID == id).Select(x => new
                {
                    x.Import_Details_ID,
                    x.Product.Product_Name,
                    x.Price,
                    x.Quantity,
                    x.Into_Money
                }).ToList();
                if (item != null)
                {
                    return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);

            }
        }
        public JsonResult Import_Delete(int id)
        {

            var item_ImportDetails = db.Import_Details.Where(x => x.Import_ID == id).ToList();
            if (item_ImportDetails.Count > 0)
            {
                foreach (var item in item_ImportDetails)
                {
                    var itemQuantity = db.Products.Where(i => i.Product_ID == item.Product_ID).SingleOrDefault();
                    if (itemQuantity != null)
                    {
                        if (itemQuantity.Quantity - item.Quantity >= 0)
                        {
                            itemQuantity.Quantity -= item.Quantity;
                            db.Import_Details.Remove(item);
                        }
                        else
                        {
                            return Json(new { status = false, msg = "Sản phẩm " + itemQuantity.Product_Name +" không thể hoàn trả do số lượng không đủ !!!. Số lượng còn lại: " + itemQuantity.Quantity });
                        }
                    }
                }
                db.SaveChanges();
            }

            var item_Import = db.Imports.Where(x => x.Import_ID == id).SingleOrDefault();
            if (item_Import != null)
            {
                db.Imports.Remove(item_Import);
                db.SaveChanges();
                return Json(new { status = true });
            }
            return Json(new { status = false, msg = "Dòng cuối cùng trong Controller" });
        }
        public JsonResult Delete_Details_Row(int id)
        {
            var item = db.Import_Details.Where(x => x.Import_Details_ID == id).SingleOrDefault();

            var import = db.Imports.Where(i => i.Import_ID == item.Import_ID).SingleOrDefault();

            var product = db.Products.Where(p => p.Product_ID == item.Product_ID).SingleOrDefault();

            if (product.Quantity - item.Quantity >= 0)
            {
                import.Import_Total -= item.Into_Money;
                product.Quantity -= item.Quantity;
                db.Import_Details.Remove(item);
                db.SaveChanges();
                return Json(new { status = true });

            }
            else
            {
                return Json(new { status = false, msg = "Sản phẩm này không thể hoàn trả do số lượng không đủ. Số lượng còn lại: " + product.Quantity });

            }
        }
        #endregion

        #region Invoice
        public ActionResult CreateInvoice()
        {
            List<Product> listPro = db.Products.ToList();
            ViewBag.listPro = new SelectList(listPro, "Product_ID", "Product_Name");
            ViewBag.ProID = (from obj in db.Products
                             select new SelectListItem()
                             {
                                 Text = obj.Product_Name,
                                 Value = obj.Product_ID.ToString()
                             }).ToList();

            var item = db.Categories.ToList();
            ViewBag.listCategory = item;

            var itemProvider = db.Providerrs.ToList();
            ViewBag.listProvider = itemProvider;
            return View();
        }

        public JsonResult GetProList_ByCate(int Cate_ID)
        {
            try
            {
                if (Cate_ID != 0)
                {
                    var item = db.Products.Where(x => x.Category_ID == Cate_ID).Select(x => new
                    {
                        x.Product_ID,
                        x.Product_Name
                    }).ToList();
                    return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var item = db.Products.ToList().Select(x => new
                    {
                        x.Product_ID,
                        x.Product_Name
                    }).ToList(); ;
                    return Json(new { status = true, data = item }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetSingleProduct(int Product_ID)
        {
            try
            {
                if (Product_ID == 0)
                {
                    var item = db.Products.Select(x => new
                    {
                        x.Product_ID,
                        x.Product_Name
                    }).ToList();
                    return Json(new { status = true, data = item, quantity = item.Count }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var item = db.Products.Where(x => x.Product_ID == Product_ID).Select(x => new
                    {
                        x.Price,
                        x.Image
                    }).SingleOrDefault();
                    return Json(new { status = true, data = item, quantity = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<GioHang> Invoice_LayGiohang()
        {
            List<GioHang> lstGioHang = (List<GioHang>)Session["GioHang_Invoice"];
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang_Invoice"] = lstGioHang;
            }
            return lstGioHang;
        }

        public JsonResult AddProduct_InvoiceCart(int Product_ID, int Quantity)
        {
            try
            {
                List<GioHang> lstGioHang = Invoice_LayGiohang();
                GioHang SanPham = lstGioHang.Find(sp => sp.iProduct_ID == Product_ID);

                if (SanPham == null)
                {
                    SanPham = new GioHang(Product_ID);
                    lstGioHang.Add(SanPham);
                    SanPham.iProduct_SoLuong = Quantity;
                    return Json(new { status = true });
                }
                else
                {
                    SanPham.iProduct_SoLuong += Quantity;
                    return Json(new { status = true });

                }
            }
            catch
            {
                return Json(new { status = false });
            }

        }

        public JsonResult DeleteProduct_InvoiceCart(int id)
        {
            try
            {
                List<GioHang> lstGioHang = Invoice_LayGiohang();
                lstGioHang.RemoveAll(sp => sp.iProduct_ID == id);
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

        public JsonResult ClearSession()
        {
            Session["GioHang_Invoice"] = null;
            return Json(new
            {
                status = true,
            });
        }

        public JsonResult AddInvoice_Database(Import import)
        {
            try
            {
                //------------------------------------------------

                Import newImp = new Import();
                newImp.Importer = import.Importer;
                newImp.Import_Date = import.Import_Date;
                newImp.Import_Total = import.Import_Total;
                newImp.Import_Note = import.Import_Note;
                newImp.Provider_ID = import.Provider_ID;
                db.Imports.Add(newImp);
                db.SaveChanges();

                //------------------------------------------------

                List<GioHang> lstGioHang = (List<GioHang>)Session["GioHang_Invoice"];
                foreach (var item in lstGioHang)
                {
                    Import_Details itemImp = new Import_Details();
                    itemImp.Import_ID = newImp.Import_ID;
                    itemImp.Product_ID = item.iProduct_ID;
                    itemImp.Quantity = item.iProduct_SoLuong;
                    itemImp.Price = item.dProduct_DonGia;
                    itemImp.Into_Money = Convert.ToDecimal(item.dProduct_ThanhTien);
                    db.Import_Details.Add(itemImp);

                    var product = db.Products.Where(x => x.Product_ID == itemImp.Product_ID).SingleOrDefault();
                    if (product != null)
                    {
                        product.Quantity += itemImp.Quantity;
                    }

                    db.SaveChanges();

                    //--------------------------------------------------------------

                    Session["GioHang_Invoice"] = null;
                }
                return Json(new { status = true, });
            }
            catch
            {
                return Json(new
                {
                    status = false,
                });
            }

        }

        #endregion
    }
}