using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watch_Shop.Models;
namespace Watch_Shop.Controllers
{
    public class ExportExcelController : Controller
    {
        Shop_DongHoEntities db = new Shop_DongHoEntities();
        // GET: ExportExcel
        public ActionResult ExprortExcelFile()
        {


            var listItem = db.Orders.ToList().OrderByDescending(x => x.Order_ID);

            int row = 2;
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            ExcelPackage excelPackage = new ExcelPackage(); 

            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Report");
            excelWorksheet.Cells["A1"].Value = "Order_ID";
            excelWorksheet.Cells["B1"].Value = "Name";
            excelWorksheet.Cells["C1"].Value = "Email";
            excelWorksheet.Cells["D1"].Value = "Phone";
            excelWorksheet.Cells["E1"].Value = "Address";
            excelWorksheet.Cells["F1"].Value = "Create_Date";
            excelWorksheet.Cells["G1"].Value = "City";
            excelWorksheet.Cells["H1"].Value = "Note";
            excelWorksheet.Cells["I1"].Value = "Total";
            excelWorksheet.Cells["J1"].Value = "Point";
            excelWorksheet.Cells["K1"].Value = "Discount";

            foreach (var item in listItem)
            {
                excelWorksheet.Cells[string.Format("A{0}", row)].Value = item.Order_ID;
                excelWorksheet.Cells[string.Format("B{0}", row)].Value = item.Name;
                excelWorksheet.Cells[string.Format("C{0}", row)].Value = item.Email;
                excelWorksheet.Cells[string.Format("D{0}", row)].Value = item.Phone;
                excelWorksheet.Cells[string.Format("E{0}", row)].Value = item.Address;
                excelWorksheet.Cells[string.Format("F{0}", row)].Value = item.Create_Date.Value.ToString();
                excelWorksheet.Cells[string.Format("G{0}", row)].Value = item.City;
                excelWorksheet.Cells[string.Format("H{0}", row)].Value = item.Note;
                excelWorksheet.Cells[string.Format("I{0}", row)].Value = (int)item.Total;
                excelWorksheet.Cells[string.Format("J{0}", row)].Value = (item.Point == null ? 0 : item.Point);
                excelWorksheet.Cells[string.Format("K{0}", row)].Value = item.Discount;

                row++;
            }

            excelWorksheet.Cells["A:AZ"].AutoFitColumns();
            excelWorksheet.Cells["A:AZ"].Style.Font.Bold.ToString();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename =" + "Report.xlsx");
            Response.BinaryWrite(excelPackage.GetAsByteArray());
            Response.End();

            return RedirectToAction("Index", "Page404_");

        }
    }
}