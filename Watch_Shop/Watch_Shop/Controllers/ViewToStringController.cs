using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Watch_Shop.Models;

namespace Watch_Shop.Controllers
{
    public class ViewToStringController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: ViewToString

        public string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writter = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);

                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new ViewToStringController());

                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writter);
                razorViewResult.View.Render(viewContext, writter);
                return writter.ToString();
            }
        }

    }
}