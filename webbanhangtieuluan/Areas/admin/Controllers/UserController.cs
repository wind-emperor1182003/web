using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
using webbanhangtieuluan.App_Start;
namespace webbanhangtieuluan.Areas.admin.Controllers
{
    public class UserController : Controller
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        //
        // GET: /admin/User/
        public ActionResult Login()
        {
            return View();
        }
        private bool userCoQuyenAdmin(taikhoanadmin user)
        {
            // Kiểm tra xem có bất kỳ bản ghi nào trong bảng PhanQuyen của người dùng có quyền admin không
            // Trả về true nếu có, ngược lại trả về false
            return user.PhanQuyens.Any(pq => pq.MaChucNang == "admin");
        }
        [HttpPost]
        public ActionResult Login(string name, string pass)
        {
            //mapTaiKhoan map = new mapTaiKhoan();
            //var user = map.TimKiem(name, pass);
            //if (user != null)
            //{
             
            //    if (userCoQuyenAdmin(user)) // Kiểm tra xem người dùng có quyền admin không
            //    {
            //        SessionConfig.SetUser(user);
            //        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            //    }
            //    else // Nếu không phải admin, giả sử là khách hàng
            //    {
            //        return RedirectToAction("Index", "home", new { area = "" }); // Điều hướng đến trang khách hàng
            //    }
            //}
            //else // Nếu không tìm thấy người dùng hoặc thông tin đăng nhập không hợp lệ
            //{
            //    ViewBag.error = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
            //    return View();
            //}
            mapTaiKhoan map = new mapTaiKhoan();
            var user = map.TimKiem(name, pass);
            if (user != null)
            {
                SessionConfig.SetUser(user);
                var us = SessionConfig.GetUser();
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            ViewBag.error = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
            return View();
        }
        public ActionResult DangKy()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangKy(taikhoanadmin model)
        {
            mapTaiKhoan map = new mapTaiKhoan();
            if (map.Themmoi(model))
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("TaiKhoanweb", "Tài khoản này đã tồn tại.");
                return View(model);
            }
        }
        public ActionResult edittaikhoan(int ms)
        {
            taikhoanadmin sp = db.taikhoanadmins.SingleOrDefault(k => k.MaTK == ms);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);

        }
         [HttpPost]
    [ValidateAntiForgeryToken]
        public ActionResult edittaikhoan([Bind(Include = "MaTK,HoTen,NgaySinh,GioiTinh,DienThoai,TaiKhoanweb,MatKhau,Email,DiaChi")] taikhoanadmin khachHang)
        {
            if (ModelState.IsValid)
            {
                var khachHangInDb = db.taikhoanadmins.SingleOrDefault(k => k.MaTK == khachHang.MaTK);
                if (khachHangInDb == null)
                {
                    return HttpNotFound();
                }

                khachHangInDb.HoTen = khachHang.HoTen;
                khachHangInDb.NgaySinh = khachHang.NgaySinh;
                khachHangInDb.GioiTinh = khachHang.GioiTinh;
                khachHangInDb.DienThoai = khachHang.DienThoai;
                khachHangInDb.TaiKhoanweb = khachHang.TaiKhoanweb;
                khachHangInDb.MatKhau = khachHang.MatKhau;
                khachHangInDb.Email = khachHang.Email;
                khachHangInDb.DiaChi = khachHang.DiaChi;

                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }
         public ActionResult XoaTaiKhoan(int ms)
         {
             var phanQuyens = db.PhanQuyens.Where(pq => pq.MaTK == ms);
             db.PhanQuyens.DeleteAllOnSubmit(phanQuyens);
             db.SubmitChanges();

             // Tìm kiếm tài khoản cần xóa trong cơ sở dữ liệu
             var taiKhoan = db.taikhoanadmins.SingleOrDefault(k => k.MaTK == ms);

             // Kiểm tra nếu không tìm thấy tài khoản
             if (taiKhoan == null)
             {
                 return HttpNotFound(); // Trả về trang lỗi 404 Not Found
             }

             // Xóa tài khoản khỏi cơ sở dữ liệu
             db.taikhoanadmins.DeleteOnSubmit(taiKhoan);
             db.SubmitChanges();

             // Chuyển hướng người dùng đến trang danh sách tài khoản hoặc trang chính của ứng dụng
             return RedirectToAction("Index", "Dashboard", new { area = "admin" }); // Điều hướng về trang Dashboard hoặc trang chính của ứng dụng
         }
         public ActionResult details(int ms)
         {
             taikhoanadmin sp = db.taikhoanadmins.SingleOrDefault(k => k.MaTK == ms);
             if (sp == null)
             {
                 return HttpNotFound();
             }
             return View(sp);
         }
}
	}
