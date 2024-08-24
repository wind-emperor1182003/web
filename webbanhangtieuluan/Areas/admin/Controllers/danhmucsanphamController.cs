using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.App_Start;
using webbanhangtieuluan.Models;
using System.IO;
namespace webbanhangtieuluan.Areas.admin.Controllers
{
    public class danhmucsanphamController : Controller
    {
        //
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        // GET: /admin/danhmucsanpham/
        [roleused(MaChucNang = "DSSANPHAM")]
        public ActionResult dssanpham()
        {
            var products = (from p in db.SanPhams select p).ToList();

            // Trả về view và chuyển danh sách sản phẩm đến view
            return View(products);
        }
        [roleused(MaChucNang = "SANPHAM")]
        public ActionResult Create()
        {
            ViewBag.huonghieu = new SelectList(db.HuongHieus.ToList(), "MaThuongHieu", "TenThuongHieu");
            ViewBag.loai = new SelectList(db.LoaiSanPhams.ToList(), "MaLoai", "TenLoaiSanPham");
            return View();

        }
        [HttpPost]
        [roleused]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham product)
        {
            ViewBag.huonghieu = new SelectList(db.HuongHieus.ToList(), "MaThuongHieu", "TenThuongHieu");
            ViewBag.loai = new SelectList(db.LoaiSanPhams.ToList(), "MaLoai", "TenLoaiSanPham");
            if (ModelState.IsValid)
            {
                db.SanPhams.InsertOnSubmit(product);
                db.SubmitChanges();

                //ViewBag.NhanHieu = new SelectList(db.Nhanhieus.ToList(), "manhanhieu", "tennhanhieu");
                //ViewBag.DanhMucSanPham = new SelectList(db.danhmucsanphams.ToList(), "Id", "TieuDe");
                return RedirectToAction("dssanpham");
            }
            return View(product);
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase IMG)
        {

            if (IMG != null && IMG.ContentLength > 0)
            {
                var fileName = Path.GetFileName(IMG.FileName);
                var path = Path.Combine(Server.MapPath("~/IMG"), fileName);
                IMG.SaveAs(path);
                return Json(new { success = true, url = Url.Content("~/IMG/" + fileName) });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult DeleteImage(string imageUrl)
        {
            try
            {
                var fileName = Path.GetFileName(imageUrl);
                var path = Path.Combine(Server.MapPath("~/IMG"), fileName);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "File not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
         [roleused(MaChucNang = "SANPHAM")]
        public ActionResult Delete(int id)
        {
            var productToDelete = db.SanPhams.FirstOrDefault(p => p.MaSanPham == id);
            if (productToDelete != null)
            {
                db.SanPhams.DeleteOnSubmit(productToDelete);
                db.SubmitChanges();
                return RedirectToAction("dssanpham");
            }
            return HttpNotFound();
        }
         [roleused(MaChucNang = "SANPHAM")]
        public ActionResult edits(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(k => k.MaSanPham == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edits([Bind(Include = "MaSanPham, TenSanPham, GiaBan, MoTa, Xuatsu, HanSuDung, AnhBia, SoLuongTon")] SanPham sanpham)
        {
            if (ModelState.IsValid)
            {
                var sanPhamInDb = db.SanPhams.SingleOrDefault(s => s.MaSanPham == sanpham.MaSanPham);
                if (sanPhamInDb == null)
                {
                    return HttpNotFound();
                }

                sanPhamInDb.TenSanPham = sanpham.TenSanPham;
                sanPhamInDb.GiaBan = sanpham.GiaBan;
                sanPhamInDb.MoTa = sanpham.MoTa;
                sanPhamInDb.Xuatsu = sanpham.Xuatsu;
                sanPhamInDb.HanSuDung = sanpham.HanSuDung;
                sanPhamInDb.AnhBia = sanpham.AnhBia;
                sanPhamInDb.SoLuongTon = sanpham.SoLuongTon;
              

                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sanpham);
        }
         [roleused(MaChucNang = "SANPHAM")]
        public ActionResult Details(int id)
        {
            var product = db.SanPhams.SingleOrDefault(p => p.MaSanPham == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
    }

}
