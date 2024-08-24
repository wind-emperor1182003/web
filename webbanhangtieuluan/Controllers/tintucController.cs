using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
using PagedList;
namespace webbanhangtieuluan.Controllers
{
    public class tintucController : Controller
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        //
        // GET: /tintuc/
        public ActionResult loaitinPartial()
        {
            var ListCD = db.loaitins.ToList();
            return View(ListCD);
        }
        public ActionResult alltintuc(int? page)
        {
            int pageSize = 2; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1);
            var products = db.tintucs.OrderBy(s => s.Matin).ToList();
            var paginatedProducts = products.ToPagedList(pageNumber, pageSize);
            return View(paginatedProducts);
        }
        public ActionResult ShowToTinTuc(int maTT)
        {
            var loaiTin = db.loaitins.FirstOrDefault(lt => lt.MLTin == maTT);

            if (loaiTin == null)
            {
                ViewBag.TB = "Không có tin tức nào";
            }
            else
            {
                ViewBag.TB = loaiTin.TLTin;
            }
            var list = db.tintucs.Where(s => s.MLTin == maTT).ToList();
            return View(list);
        }
        public ActionResult XemChitiettt(int maTT)
        {
            tintuc sp = db.tintucs.Single(s => s.Matin == maTT);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);

        }
	}
}