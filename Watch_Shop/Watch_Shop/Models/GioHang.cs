using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Watch_Shop.Models;
using System.Data.Entity;

namespace Watch_Shop.Models
{
    public class GioHang
    {
        Watch_Shop.Models.Shop_DongHoEntities db = new Shop_DongHoEntities();
        public int iProduct_ID { get; set; }
        public string sProduct_Name { get; set; }
        public string sProduct_Image { get; set; }
        public decimal ? dProduct_DonGia { get; set; }
        public int iProduct_SoLuong { get; set; }
        public double dProduct_ThanhTien
        {
            get { return iProduct_SoLuong * Convert.ToDouble(dProduct_DonGia); }
        }
        public GioHang(int Product_ID)
        {

            iProduct_ID = Product_ID;
            Product sp = db.Products.SingleOrDefault(s => s.Product_ID == iProduct_ID);
            sProduct_Name = sp.Product_Name;
            sProduct_Image = sp.Image;
            dProduct_DonGia = sp.Price;
            iProduct_SoLuong = 1;
        }
        public GioHang()
        {
            //    this.iProduct_ID = 1;
            //    this.sProduct_Name = "";
            //    this.sProduct_Image = "";
            //    this.dProduct_DonGia = 1;
            //    this.iProduct_SoLuong = 1;
        }
    }
}