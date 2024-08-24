using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.App_Start;
using webbanhangtieuluan.Models;
namespace webbanhangtieuluan.Areas.admin.Controllers
{
    
    public class DashboardController : Controller
    {
        //
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        // GET: /admin/Dashboard/
        [roleused]
        public ActionResult Index()
        {
            return View(new mapTaiKhoan().DanhSach());
        }
          [roleused(MaChucNang = "PHANQUYEN")]
        public ActionResult phanquyen(int idtaikhoan)
        {
            taikhoanadmin sp = db.taikhoanadmins.SingleOrDefault(k => k.MaTK == idtaikhoan);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);
        }
          [roleused(MaChucNang = "DSPHANQUYEN")]
          public ActionResult danhsachphanquyen()
          {
              return View(new mapTaiKhoan().DanhSach());
          }
        [roleused(MaChucNang = "LUUPHANQUYEN")]
          public ActionResult luuphanquyen(string idtaikhoan,string chucnang)
          {
              int idTaiKhoan = Convert.ToInt32(idtaikhoan);

              // Kiểm tra nếu chucnang không phải là null
              if (!string.IsNullOrEmpty(chucnang))
              {
                  new mapphanquyen().luuphanquyen(idTaiKhoan, chucnang);
              }

              return RedirectToAction("phanquyen", new { idtaikhoan = idTaiKhoan });
          }
	}
}