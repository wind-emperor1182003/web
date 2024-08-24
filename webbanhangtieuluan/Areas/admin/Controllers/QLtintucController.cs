using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
using webbanhangtieuluan.App_Start;
using System.IO;
namespace webbanhangtieuluan.Areas.admin.Controllers
{
    public class QLtintucController : Controller
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        //
        // GET: /admin/QLtintuc/
          [roleused(MaChucNang = "DSTT")]
        public ActionResult dstintuc()
        {
            var tintucs = db.tintucs.ToList();
            return View(tintucs);
        }
        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase IMG)
        {

            if (IMG != null && IMG.ContentLength > 0)
            {
                var fileName = Path.GetFileName(IMG.FileName);
                var path = Path.Combine(Server.MapPath("~/IMG/tintuc/"), fileName);
                IMG.SaveAs(path);
                return Json(new { success = true, url = Url.Content("~/IMG/tintuc/" + fileName) });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult DeleteImage(string imageUrl)
        {
            try
            {
                var fileName = Path.GetFileName(imageUrl);
                var path = Path.Combine(Server.MapPath("~/IMG/tintuc"), fileName);

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
          [roleused(MaChucNang = "DSTT")]
        public ActionResult Create()
        {
       
            return View();

        }
        [HttpPost]
        [roleused]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tintuc product)
        {
           
            if (ModelState.IsValid)
            {
                db.tintucs.InsertOnSubmit(product);
                db.SubmitChanges();

                //ViewBag.NhanHieu = new SelectList(db.Nhanhieus.ToList(), "manhanhieu", "tennhanhieu");
                //ViewBag.DanhMucSanPham = new SelectList(db.danhmucsanphams.ToList(), "Id", "TieuDe");
                return RedirectToAction("dstintuc");
            }
            return View(product);
        }
          [roleused(MaChucNang = "DSTT")]
        public ActionResult Delete(int id)
        {
            var taiKhoan = db.tintucs.FirstOrDefault(t => t.Matin == id);
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
            var taiKhoan = db.tintucs.FirstOrDefault(t => t.Matin == id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }



            db.tintucs.DeleteOnSubmit(taiKhoan);
            db.SubmitChanges();
            return RedirectToAction("dstintuc");
        }
          [roleused(MaChucNang = "DSTT")]
        public ActionResult Edit(int id)
        {
            var tintuc = db.tintucs.FirstOrDefault(t => t.Matin == id);
            if (tintuc == null)
            {
                return HttpNotFound();
            }
            return View(tintuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tintuc tintuc)
        {
            if (ModelState.IsValid)
            {
                var existingTintuc = db.tintucs.FirstOrDefault(t => t.Matin == tintuc.Matin);
                if (existingTintuc != null)
                {
                    existingTintuc.TieuDeTin = tintuc.TieuDeTin;
                    existingTintuc.NoiDung = tintuc.NoiDung;
                    existingTintuc.NgayDang = tintuc.NgayDang;
                    existingTintuc.AnhMH = tintuc.AnhMH;
                    existingTintuc.MLTin = tintuc.MLTin;

                    db.SubmitChanges();
                    return RedirectToAction("dstintuc");
                }
            }
            return View(tintuc);
        }
	}
}