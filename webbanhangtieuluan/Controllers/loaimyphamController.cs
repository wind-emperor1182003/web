using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
namespace webbanhangtieuluan.Controllers
{
    public class loaimyphamController : Controller
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        //
        //
        // GET: /loaimypham/
        public ActionResult loaiPartial()
        {
            var ListCD = db.LoaiSanPhams.ToList();
            return View(ListCD);
        }
        public ActionResult loaiPartial1()
        {
            var ListCD = db.LoaiSanPhams.ToList();
            return View(ListCD);
        }
        public ActionResult loai(int maCD)
        {
            var listSachCD = db.SanPhams.Where(s => s.MaLoai == maCD).ToList();
           LoaiSanPham cd = db.LoaiSanPhams.Single(x => x.MaLoai == maCD);
            ViewBag.TenCD = cd.TenLoaiSanPham;
            if (listSachCD == null || !listSachCD.Any())
            {
                ViewBag.ThongBao = "Không có sản phẩm";
            }
            return View(listSachCD);
        }
	}
}