using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webbanhangtieuluan.Models
{
    public class giohang
    {
        QLmyphamdbmlDataContext db=new QLmyphamdbmlDataContext();
        public int smasanpham { set; get; }
        public string stensanpham { set; get; }
        public string sanhbia { set; get; }
        public double sdongia { set; get; }
        public int ssoluong { set; get; }
        public int ssoluongton { set; get; }
        public double sthanhtien
        {
            get { return ssoluong * sdongia; }        
        }
        public giohang(int Masanpham)
        {
            smasanpham = Masanpham;
            SanPham sanpham = db.SanPhams.Single(s => s.MaSanPham == smasanpham);
            stensanpham = sanpham.TenSanPham;
            sanhbia = sanpham.AnhBia;
            sdongia = double.Parse(sanpham.GiaBan.ToString());
            ssoluong = 1;
            
        }
    }
}