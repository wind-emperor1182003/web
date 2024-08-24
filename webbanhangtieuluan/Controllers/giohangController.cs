using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
namespace webbanhangtieuluan.Controllers
{
    public class giohangController : Controller
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        //
        // GET: /giohang/

        public ActionResult Index()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "home");
            }
            List<giohang> lsgiohang = laygiohang();
            return View(lsgiohang);
        }
      [HttpPost]
public ActionResult UpdateCart(int id, int quantity)
{
    // Retrieve the list of products in the cart from Session
    List<giohang> lsgiohang = Session["GioHang"] as List<giohang>;

    // Find the product to be updated in the cart
    giohang sanpham = lsgiohang.FirstOrDefault(sp => sp.smasanpham == id);

    if (sanpham != null)
    {
        // Retrieve the product's stock quantity from the database
        var product = db.SanPhams.FirstOrDefault(sp => sp.MaSanPham == id);

        if (product != null)
        {
            // Check if the desired quantity exceeds the available stock
            if (quantity <= product.SoLuongTon)
            {
                // Update the product quantity in the cart
                sanpham.ssoluong = quantity;

                // Save the updated list of products back to the Session
                Session["GioHang"] = lsgiohang;

                // Save success notification to TempData
                TempData["UpdateSuccessMessage"] = "Giỏ hàng đã được cập nhật thành công.";
            }
            else
            {
                // Save error notification to TempData
                TempData["UpdateErrorMessage"] = "Số lượng cập nhật vượt quá số lượng tồn kho.";
            }
        }
        else
        {
            // Save error notification to TempData if the product does not exist in the database
            TempData["UpdateErrorMessage"] = "Sản phẩm không tồn tại.";
        }
    }
    else
    {
        // Save error notification to TempData if the product is not in the cart
        TempData["UpdateErrorMessage"] = "Sản phẩm không tồn tại trong giỏ hàng.";
    }

    // Redirect to the appropriate action/view
    return RedirectToAction("Index");
}



        public List<giohang> laygiohang()
        {
            List<giohang> lsgiohang = Session["GioHang"] as List<giohang>;
            if (lsgiohang == null)
            {
                lsgiohang = new List<giohang>();
                Session["GioHang"] = lsgiohang;
            }
            return lsgiohang;
        }
        //public ActionResult themgiohang(int ms, string strURL)
        //{
        //    List<giohang> lsgiohang = laygiohang();
        //    giohang sanpham = lsgiohang.Find(sp => sp.smasanpham == ms);
        //    if (sanpham == null)
        //    {
        //        sanpham = new giohang(ms);
        //        lsgiohang.Add(sanpham);
        //    }
        //    else
        //    {
        //        sanpham.ssoluong++;
        //    }

        //    // Chuyển hướng đến phương thức XemChitiet của sanphamController với tham số ms
        //    return RedirectToAction("XemChitiet", "sanpham", new { ms = ms });
        //}
       public ActionResult themgiohang(int ms, string strURL)
{
    // Get the product from the database
    var product = db.SanPhams.FirstOrDefault(sp => sp.MaSanPham == ms);
    
    // Check if the product exists and if it is in stock
    if (product != null && product.SoLuongTon > 0)
    {
        List<giohang> lsgiohang = laygiohang();
        giohang sanpham = lsgiohang.Find(sp => sp.smasanpham == ms);
        
        if (sanpham == null)
        {
            // If the product is not in the cart, add it with quantity 1
            sanpham = new giohang(ms);
            lsgiohang.Add(sanpham);
            TempData["UpdateSuccessMessage"] = "Giỏ hàng đã thêm sản phẩm thành công.";
        }
        else
        {
            // Check if adding one more to the quantity exceeds the available stock
            if (sanpham.ssoluong <= product.SoLuongTon)
            {
                sanpham.ssoluong++; // Increase the quantity by 1
                TempData["UpdateSuccessMessage"] = "Giỏ hàng đã thêm sản phẩm thành công.";
            }
            else
            {
                // Display a message indicating that the quantity in the cart cannot exceed the available stock
                TempData["OutOfStockMessage"] = "Số lượng sản phẩm trong giỏ hàng không thể vượt quá số lượng tồn kho.";
                return RedirectToAction("XemChitiet", "sanpham", new { ms = ms });
            }
        }

        // Redirect to the product details page
        return RedirectToAction("XemChitiet", "sanpham", new { ms = ms });
    }
    else
    {
        // Product is out of stock or does not exist, handle accordingly (e.g., display a message)
        TempData["OutOfStockMessage"] = "Sản phẩm đã hết hàng hoặc không tồn tại.";
        
        // Redirect back to the previous page or another appropriate action
        return RedirectToAction("XemChitiet", "sanpham", new { ms = ms });
    }
}


        private int tongsoluong()
        {
            int tsl = 0;
            List<giohang> lstgiohang = Session["gioHang"] as List<giohang>;
            if (lstgiohang != null)
            {
                tsl += lstgiohang.Sum(sp => sp.ssoluong);

            }
            return tsl;
        }
        private double tongthangtien()
        {
            double ttt = 0;
            List<giohang> lstgiohang = Session["GioHang"] as List<giohang>;
            if (lstgiohang != null)
            {
                ttt += lstgiohang.Sum(sp => sp.sthanhtien);

            }
            return ttt;
        }

             public ActionResult Giohangpartial()
        {
            ViewBag.Tongsoluong = tongsoluong();
            ViewBag.Tongthanhtien = tongthangtien();
            
            return PartialView();
        }
             public ActionResult RemoveFromCart(int id)
             {
                 List<giohang> lsgiohang = laygiohang();
                 giohang sanpham = lsgiohang.Find(sp => sp.smasanpham == id);
                 if (sanpham != null)
                 {
                     lsgiohang.Remove(sanpham);
                     TempData["RemoveSuccessMessage"] = "Sản phẩm đã được xóa khỏi giỏ hàng.";
                 }
                 return RedirectToAction("Index");
             }
             public ActionResult ThanhToan()
             {
                 return View();
             }

             private bool GuiEmailXacNhanDonHang(taikhoan khachHang, DonHang donHang, List<giohang> gioHang)
             {
                 try
                 {
                     var confirmLink = Url.Action("XacNhanDonHang", "giohang", new { id = donHang.MaDonHang }, Request.Url.Scheme);

                     var smtpServer = "smtp.gmail.com";
                     var port = 587;
                     var userName = "Hoangphong1182003@gmail.com";
                     var password = "isfs kaiz qose yfoo"; // Thay bằng mật khẩu ứng dụng của bạn

                     var toEmail = khachHang.Email;

                     var mail = new MailMessage();
                     mail.From = new MailAddress(userName);
                     mail.To.Add(toEmail);
                     mail.Subject = "Xác nhận đơn hàng";

                     var body = new StringBuilder();
                     body.AppendLine("<html><head>");
                     body.AppendLine("<style>");
                     body.AppendLine("@import url('https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css');");
                     body.AppendLine("</style>");
                     body.AppendLine("</head><body>");
                     body.AppendLine("<div class='container'>");
                     body.AppendLine("<h3 class='mt-4'>Thông tin đặt hàng</h3>");
                     body.AppendLine("<div class='row'>");
                     body.AppendLine("<div class='col-md-6'>");
                     body.AppendLine("<p class='font-weight-bold'>Họ và tên khách hàng:</p>");
                     body.AppendLine("<p>" + khachHang.HoTen + "</p>");
                     body.AppendLine("</div>");
                     body.AppendLine("<div class='col-md-6'>");
                     body.AppendLine("<p class='font-weight-bold'>Số điện thoại:</p>");
                     body.AppendLine("<p>" + khachHang.DienThoai + "</p>");
                     body.AppendLine("</div>");
                     body.AppendLine("<div class='col-md-12'>");
                     body.AppendLine("<p class='font-weight-bold'>Địa chỉ:</p>");
                     body.AppendLine("<p>" + khachHang.DiaChi + "</p>");
                     body.AppendLine("</div>");
                     body.AppendLine("</div>");
                     body.AppendLine("<p class='mt-4'>Xin chào <strong>" + khachHang.HoTen + "</strong>,</p>");
                     body.AppendLine("<p>Cảm ơn bạn đã đặt hàng tại cửa hàng của chúng tôi. Dưới đây là thông tin đơn hàng của bạn:</p>");
                     DateTime ngayDat = Convert.ToDateTime(donHang.NgayDat);
                     body.AppendLine("<p>Ngày đặt hàng: " + ngayDat.ToString("dd/MM/yyyy") + "</p>");
                     body.AppendLine("<p>Mã đơn hàng: " + donHang.MaDonHang + "</p>");
                     body.AppendLine("<p class='font-weight-bold'>Danh sách sản phẩm:</p>");
                     body.AppendLine("<ul>");
                     foreach (var item in gioHang)
                     {
                         body.AppendLine("<li>" + item.smasanpham + ": " + item.ssoluong + " x " + item.sdongia.ToString("C") + "</li>");
                     }
                     body.AppendLine("</ul>");
                     body.AppendLine("<p class='font-weight-bold'>Tổng số tiền: " + tongthangtien().ToString("C") + "</p>");
                     body.AppendLine("<p>Chúng tôi đã nhận được đơn hàng của bạn. Vui lòng nhấn vào <a href='" + confirmLink + "'>Xác nhận đơn hàng</a> để hoàn thành quá trình đặt hàng.</p>");
                     body.AppendLine("<p>Chúng tôi sẽ liên hệ với bạn sớm nhất để xác nhận đơn hàng.</p>");
                     DateTime ngayGiao = ngayDat.AddDays(2);
                     body.AppendLine("<p>Thời gian giao hàng: " + ngayGiao.ToString("dd/MM/yyyy") + "</p>");
                     body.AppendLine("<p>Trân trọng,</p>");
                     body.AppendLine("<p>Cửa hàng Mỹ Phẩm</p>");
                     body.AppendLine("</div>");

                     body.AppendLine("</body></html>");
                     mail.Body = body.ToString();
                     mail.IsBodyHtml = true;

                     var smtpClient = new SmtpClient(smtpServer, port);
                     smtpClient.Credentials = new NetworkCredential(userName, password);
                     smtpClient.EnableSsl = true;

                     smtpClient.Send(mail);

                     Console.WriteLine("Email đã được gửi thành công đến: " + khachHang.Email);
                     return true;
                 }
                 catch (SmtpException smtpEx)
                 {
                     Console.WriteLine("SMTP Exception: " + smtpEx.Message);
                     if (smtpEx.InnerException != null)
                     {
                         Console.WriteLine("Inner Exception: " + smtpEx.InnerException.Message);
                     }
                     return false;
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine("Exception: " + ex.Message);
                     if (ex.InnerException != null)
                     {
                         Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                     }
                     return false;
                 }
             }


             [HttpPost]
             public ActionResult ThanhToan(taikhoan khachHang)
             {


                 if (ModelState.IsValid)
                 {
                     using (var db = new QLmyphamdbmlDataContext())
                     {
                         try
                         {
                             db.taikhoans.InsertOnSubmit(khachHang);
                             db.SubmitChanges();

                             var donHang = new DonHang
                             {
                                 NgayDat = DateTime.Now,
                                 DaThanhToan = "Chưa",
                                 TinhTrangGiaoHang = 0,
                                 MaKH = khachHang.MaKH
                             };

                             db.DonHangs.InsertOnSubmit(donHang);
                             db.SubmitChanges();

                             var gioHang = Session["GioHang"] as List<giohang>;
                             if (gioHang != null)
                             {
                                 foreach (var item in gioHang)
                                 {
                                     var chiTietDonHang = new ChiTietDonHang
                                     {
                                         MaDonHang = donHang.MaDonHang,
                                         MaSanpham = item.smasanpham,
                                         SoLuong = item.ssoluong,
                                         DonGia = (decimal?)item.sdongia
                                     };

                                     db.ChiTietDonHangs.InsertOnSubmit(chiTietDonHang);
                                 }

                                 db.SubmitChanges();

                                 if (GuiEmailXacNhanDonHang(khachHang, donHang, gioHang))
                                 {
                                     Session["GioHang"] = null; // Xóa giỏ hàng sau khi thanh toán thành công
                                     return RedirectToAction("ThanhToan");
                                 }
                                 else
                                 {
                                     ModelState.AddModelError("", "Đặt hàng thành công nhưng gửi email xác nhận thất bại.");
                                 }
                             }
                         }
                         catch (Exception ex)
                         {
                             ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu thông tin khách hàng: " + ex.Message);
                         }
                     }
                 }

                 return View(khachHang);
             }
             //[HttpPost]
             //public ActionResult ThanhToan(taikhoan khachHang)
             //{
             //    if (ModelState.IsValid)
             //    {
             //        // Khởi tạo đối tượng DbContext

             //        try
             //        {
             //            // Thêm khách hàng mới vào bảng KhachHang
             //            db.taikhoans.InsertOnSubmit(khachHang);
             //            // Lưu thay đổi vào cơ sở dữ liệu
             //            db.SubmitChanges();

             //            // Xử lý đặt hàng
             //            // ...
                         

             //            // Chuyển hướng đến trang cảm ơn
             //            return RedirectToAction("CamOn");
             //        }
             //        catch (Exception ex)
             //        {
             //            // Xử lý các ngoại lệ nếu cần
             //            ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu thông tin khách hàng: " + ex.Message);
             //        }
             //    }


             //    // Nếu dữ liệu không hợp lệ hoặc xử lý lưu không thành công, hiển thị lại trang thanh toán
             //    return View(khachHang);
             //}

             // Action method để hiển thị trang cảm ơn sau khi đặt hàng thành công
             public ActionResult CamOn()
             {
                 return View();
             }
            
             public ActionResult XacNhanDonHang(int id)
             {
                 // Tìm đơn hàng trong cơ sở dữ liệu dựa trên id
                 var donHang = db.DonHangs.FirstOrDefault(dh => dh.MaDonHang == id);

                 if (donHang != null)
                 {
                     // Đánh dấu đơn hàng đã được thanh toán
                     donHang.DaThanhToan = "Đã thanh toán";
                     db.SubmitChanges();

                     // Chuyển hướng đến trang hiển thị chi tiết đơn hàng đã được xác nhận
                     return RedirectToAction("CamOn");
                 }

                 // Nếu không tìm thấy đơn hàng, có thể chuyển hướng đến trang lỗi
                 return View("Error");
             }
        }

        
	}
