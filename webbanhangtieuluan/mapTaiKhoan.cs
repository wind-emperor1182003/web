using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webbanhangtieuluan.Models;

namespace webbanhangtieuluan
{
    public class mapTaiKhoan
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        public taikhoanadmin TimKiem(string usernames, string password)
        {
            var user = db.taikhoanadmins.Where(x => x.TaiKhoanweb == usernames & x.MatKhau == password).ToList();
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }
        }
        public taikhoanadmin TimKiem(int id)
        {
            var user = db.taikhoanadmins.SingleOrDefault(u => u.MaTK == id);
           
                return user;
           
        }
        public List<taikhoanadmin> DanhSach()
        {
            var user = db.taikhoanadmins.ToList();
            return user;

        }
        public bool Themmoi(taikhoanadmin tk)
        {
            try
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                var existingUser = db.taikhoanadmins.FirstOrDefault(t => t.TaiKhoanweb == tk.TaiKhoanweb);
                if (existingUser != null)
                {
                    // Nếu tài khoản đã tồn tại, trả về false
                    return false;
                }

                // Thêm tài khoản mới vào bảng taikhoanadmins của cơ sở dữ liệu
                db.taikhoanadmins.InsertOnSubmit(tk);

                // Lưu các thay đổi vào cơ sở dữ liệu
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi (bỏ ghi chú dòng sau đây sau khi thêm cơ chế ghi nhật ký)
                // Console.WriteLine(ex.Message);
                return false;
            }
        }


    }
}