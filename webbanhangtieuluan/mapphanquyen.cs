using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webbanhangtieuluan.Models;

namespace webbanhangtieuluan
{
    public class mapphanquyen
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        public string mesage = "";
        public bool luuphanquyen(int idtaikhoan, string idchucnang)
        {
            var role = db.PhanQuyens.SingleOrDefault(x => x.MaTK == idtaikhoan && x.MaChucNang == idchucnang);
            if (role != null)
            {
                db.PhanQuyens.DeleteOnSubmit(role);
                db.SubmitChanges();
                return true;
            }
            else
            {
                var role2 = new PhanQuyen
                {
                    MaTK = idtaikhoan,
                    MaChucNang = idchucnang
                };
                db.PhanQuyens.InsertOnSubmit(role2);
                db.SubmitChanges();
                return true;
            }
        }

        public bool KiemTra(int idtaikhoan, string idchucnang)
        {
            var dem = db.PhanQuyens.Count(x => x.MaTK == idtaikhoan && x.MaChucNang == idchucnang);
            if (dem > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<ChucNang> danhsachchucnang()
        {
            return db.ChucNangs.ToList();
        }
    }
}