using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;
namespace Watch_Shop.Controllers
{
    public class ContactController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendFeedback(Contact ct)
        {
            try
            {
                // Status = 0: Not Seen ------ Status = 1: Seen
                ct.Create_Date = DateTime.Now;
                ct.Status = 0;
                db.Contacts.Add(ct);
                db.SaveChanges();
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });
            }
        }

        public ActionResult ListContact_Partial()
        {
            var item = db.Contacts.ToList().OrderByDescending(x=>x.Create_Date);
            return PartialView(item);
        }

        public JsonResult Delete_Contact(int id)
        {
            var item = db.Contacts.Where(x => x.Contact_ID == id).SingleOrDefault();
            if(item != null)
            {
                db.Contacts.Remove(item);
                db.SaveChanges();
                return Json(new { status = true });
            }
            return Json(new { status = false });

        }
    }
}