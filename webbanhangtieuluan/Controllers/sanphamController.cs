using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
using System.Web.Mvc;


namespace webbanhangtieuluan.Controllers
{
    public class sanphamController : Controller
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        //
        // GET: /sanpham/
        public ActionResult sanphamall()
        {
            return View(db.SanPhams.OrderBy(s => s.TenSanPham).ToList());
        }
        public ActionResult all(int? page)
        {
            int pageSize = 4; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1);
            var products = db.SanPhams.OrderBy(s => s.TenSanPham).ToList();
            var paginatedProducts = products.ToPagedList(pageNumber, pageSize);

            return View(paginatedProducts);
        }
        public ActionResult XemChitiet(int ms)
        {
           SanPham sp = db.SanPhams.Single(s => s.MaSanPham == ms);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);

        }
        public ActionResult TimSach(string txt_Search)
        {
         
            List<SanPham> ListSach;
            if (!String.IsNullOrEmpty(txt_Search))
            {
                ListSach = db.SanPhams.Where(x => x.TenSanPham.ToUpper().Contains(txt_Search.ToUpper())).OrderBy(x => x.TenSanPham).ToList();
                ViewBag.SoLuongSP = "Tìm thấy " + ListSach.Count + " sản phẩm với từ khóa " + "\"" + txt_Search + "\"";
            }
            else
            {
                ListSach = new List<SanPham>(); // Khởi tạo danh sách rỗng
                ViewBag.SoLuongSP = "Có tất cả " + db.SanPhams.Count() + " sản phẩm ";
            }
            return View(ListSach);
        }
        //public ActionResult TimSach(string txt_Search, int? page)
        //{
        //    int pageSize = 4; // Số lượng sản phẩm trên mỗi trang
        //    int pageNumber = (page ?? 1);

        //    List<SanPham> ListSach;
        //    if (!String.IsNullOrEmpty(txt_Search))
        //    {
        //        // Tìm kiếm sản phẩm theo từ khóa và sắp xếp theo tên
        //        ListSach = db.SanPhams
        //                      .Where(x => x.TenSanPham.ToUpper().Contains(txt_Search.ToUpper()))
        //                      .OrderBy(x => x.TenSanPham)
        //                      .ToList();

        //        ViewBag.SoLuongSP = "Tìm thấy " + ListSach.Count + " sản phẩm với từ khóa " + "\"" + txt_Search + "\"";
        //    }
        //    else
        //    {
        //        // Nếu không có từ khóa tìm kiếm, lấy tất cả sản phẩm
        //        ListSach = db.SanPhams
        //                      .OrderBy(x => x.TenSanPham)
        //                      .ToList();

        //        ViewBag.SoLuongSP = "Có tất cả " + ListSach.Count + " sản phẩm ";
        //    }

        //    // Sử dụng PagedList để phân trang
        //    var paginatedProducts = ListSach.ToPagedList(pageNumber, pageSize);

        //    return View(paginatedProducts);
        //}

	}
}