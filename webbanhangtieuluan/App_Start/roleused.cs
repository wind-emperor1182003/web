using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
namespace webbanhangtieuluan.App_Start
{
    public class roleused : AuthorizeAttribute
    {
        public string MaChucNang { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = SessionConfig.GetUser();
            if (user == null)
            {
                //rt về trang đăng nhập
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "User",
                        action = "Login",
                        area = "Admin"
                    }));
                return;
            }
            //check quyền
            if (string.IsNullOrEmpty(MaChucNang) == false)
            {
                var check = new mapphanquyen().KiemTra(user.MaTK,MaChucNang);
                if (check == false)
                {
                    filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary(new
                   {
                       controller = "Dashboard",
                       action = "LoiPhanQuyen",
                       area = "Admin"
                   }));
                    return;
                }
            }
            return;
        }


    }
}
