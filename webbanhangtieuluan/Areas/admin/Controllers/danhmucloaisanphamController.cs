using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
using webbanhangtieuluan.App_Start;
namespace webbanhangtieuluan.Areas.admin.Controllers
{
    public class danhmucloaisanphamController : Controller
    {
      
        //
        // GET: /admin/danhmucloaisanpham/
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        // GET: /admin/danhmucsanpham/
        [roleused(MaChucNang = "DSLOAISP")]
        public ActionResult dsloai()
        {
            var products = (from p in db.LoaiSanPhams select p).ToList();

            // Trả về view và chuyển danh sách sản phẩm đến view
            return View(products);
        }
         [roleused(MaChucNang = "DSLOAISP")]
        public ActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [roleused]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoaiSanPham product)
        {
            if (ModelState.IsValid)
            {
                db.LoaiSanPhams.InsertOnSubmit(product);
                db.SubmitChanges();

                //ViewBag.NhanHieu = new SelectList(db.Nhanhieus.ToList(), "manhanhieu", "tennhanhieu");
                //ViewBag.DanhMucSanPham = new SelectList(db.danhmucsanphams.ToList(), "Id", "TieuDe");
                return RedirectToAction("dsloai");
            }
            return View(product);
        }
         [roleused(MaChucNang = "DSLOAISP")]
        public ActionResult Delete(int id)
        {
            var productToDelete = db.LoaiSanPhams.FirstOrDefault(p => p.MaLoai == id);
            if (productToDelete != null)
            {
                db.LoaiSanPhams.DeleteOnSubmit(productToDelete);
                db.SubmitChanges();
                return RedirectToAction("dsloai");
            }
            return HttpNotFound();
        }
       
	}
}