using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
using webbanhangtieuluan.App_Start;
using System.Data.Linq;
using System.Net;


namespace webbanhangtieuluan.Areas.admin.Controllers
{
    public class QLkhachhangController : Controller
    {
        //
        // GET: /admin/QLkhachhang/
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        public ActionResult Index()
        {
            return View();
        }
        //tailhoan/////////////////////////////////////////////////////////////////
        [roleused(MaChucNang = "DSKH")]
        public ActionResult dskhachhang()
        {
            var taiKhoans = db.taikhoans.ToList();
            return View(taiKhoans);
        }
         [roleused(MaChucNang = "DSKH")]
        public ActionResult Details(int id)
        {
            var taiKhoan = db.taikhoans.FirstOrDefault(t => t.MaKH == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            var donHangs = db.DonHangs.Where(d => d.MaKH == id).ToList();
            ViewBag.DonHangs = donHangs;

            return View(taiKhoan);
        }
         [roleused(MaChucNang = "DSKH")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(taikhoan taiKhoan)
        {
            if (db.taikhoans.Any(t => t.TaiKhoanweb == taiKhoan.TaiKhoanweb))
            {
                ModelState.AddModelError("TaiKhoanweb", "Tài khoản đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                db.taikhoans.InsertOnSubmit(taiKhoan);
                db.SubmitChanges();
                return RedirectToAction("dskhachhang");
            }

            return View(taiKhoan);
        }
        [HttpPost]
        public JsonResult IsUsernameAvailable(string TaiKhoanweb)
        {
            return Json(!db.taikhoans.Any(t => t.TaiKhoanweb == TaiKhoanweb), JsonRequestBehavior.AllowGet);
        }
         [roleused(MaChucNang = "DSKH")]
        public ActionResult Delete(int id)
        {
            var taiKhoan = db.taikhoans.FirstOrDefault(t => t.MaKH == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var taiKhoan = db.taikhoans.FirstOrDefault(t => t.MaKH == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            var donHangs = db.DonHangs.Where(d => d.MaKH == id).ToList();
            if (donHangs.Any())
            {
                ViewBag.ErrorMessage = "Bạn phải xóa các đơn hàng liên quan trước khi xóa tài khoản này.";
                return View("Delete", taiKhoan);
            }

            db.taikhoans.DeleteOnSubmit(taiKhoan);
            db.SubmitChanges();
            return RedirectToAction("dskhachhang");
        }
         [roleused(MaChucNang = "DSKH")]
        public ActionResult Edit(int id)
        {
            var taiKhoan = db.taikhoans.FirstOrDefault(t => t.MaKH == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: admin/QLkhachhang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(taikhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var existingTaiKhoan = db.taikhoans.FirstOrDefault(t => t.MaKH == taiKhoan.MaKH);
                if (existingTaiKhoan != null)
                {
                    // Update existingTaiKhoan with data from taiKhoan
                    existingTaiKhoan.HoTen = taiKhoan.HoTen;
                    existingTaiKhoan.NgaySinh = taiKhoan.NgaySinh;
                    existingTaiKhoan.GioiTinh = taiKhoan.GioiTinh;
                    existingTaiKhoan.DienThoai = taiKhoan.DienThoai;
                    existingTaiKhoan.TaiKhoanweb = taiKhoan.TaiKhoanweb;
                    existingTaiKhoan.MatKhau = taiKhoan.MatKhau;
                    existingTaiKhoan.Email = taiKhoan.Email;
                    existingTaiKhoan.DiaChi = taiKhoan.DiaChi;

                    db.SubmitChanges();
                    return RedirectToAction("dskhachhang");
                }
            }
            // If ModelState is not valid, return to the Edit view with the provided taiKhoan model
            return View(taiKhoan);
        }
        //tailhoan///////////////////////////////////////////////////
         [roleused(MaChucNang = "DSKH")]
        public ActionResult Delete1(int id)
        {
            var taiKhoan = db.DonHangs.FirstOrDefault(t => t.MaKH == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        [HttpPost, ActionName("Delete1")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed1(int id)
        {
            var taiKhoan = db.DonHangs.FirstOrDefault(t => t.MaDonHang == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            var donHangs = db.ChiTietDonHangs.Where(d => d.MaDonHang == id).ToList();
            if (donHangs.Any())
            {
                ViewBag.ErrorMessage = "Bạn phải xóa các chi tiết đơn hàng liên quan trước khi xóa tài khoản này.";
                return View("Delete1", taiKhoan);
            }

            db.DonHangs.DeleteOnSubmit(taiKhoan);
            db.SubmitChanges();
            return RedirectToAction("dskhachhang");
        }
         [roleused(MaChucNang = "DSKH")]
        public ActionResult Details1(int id)
        {
            var donHang = db.DonHangs.FirstOrDefault(d => d.MaDonHang == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            var chiTietDonHangs = db.ChiTietDonHangs.Where(c => c.MaDonHang == id).ToList();
            ViewBag.ChiTietDonHangs = chiTietDonHangs;

            return View(donHang);
        }
        [roleused(MaChucNang = "DSKH")]
        public ActionResult dsdonhang()
        {
            var products = (from p in db.DonHangs select p).ToList();

            // Trả về view và chuyển danh sách sản phẩm đến view
            return View(products);
        }
        [roleused(MaChucNang = "DSKH")]
        public ActionResult dschitietdonhang()
        {
            var products = (from p in db.ChiTietDonHangs select p).ToList();

            // Trả về view và chuyển danh sách sản phẩm đến view
            return View(products);
        }
         [roleused(MaChucNang = "DSKH")]
        public ActionResult Delete2(int id)
        {
            var taiKhoan = db.ChiTietDonHangs.FirstOrDefault(t => t.MaDonHang == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        [HttpPost, ActionName("Delete2")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed2(int id)
        {
            var taiKhoan = db.ChiTietDonHangs.FirstOrDefault(t => t.MaDonHang == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }



            db.ChiTietDonHangs.DeleteOnSubmit(taiKhoan);
            db.SubmitChanges();
            return RedirectToAction("dskhachhang");
        }
        //public ActionResult DeleteKH(int id)
        //{
        //    var productToDelete = db.taikhoans.FirstOrDefault(p => p.MaKH == id);
        //    if (productToDelete != null)
        //    {
        //        db.taikhoans.DeleteOnSubmit(productToDelete);
        //        db.SubmitChanges();
        //        return RedirectToAction("dskhachhang");
        //    }
        //    return HttpNotFound();
        //}
     
            }
        }

	
