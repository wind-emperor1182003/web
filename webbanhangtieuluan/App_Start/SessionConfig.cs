using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webbanhangtieuluan.Models;
namespace webbanhangtieuluan.App_Start
{
    public class SessionConfig
    {
        public static void SetUser(taikhoanadmin User)
        {

            HttpContext.Current.Session["user"] = User;
        }
        public static taikhoanadmin GetUser()
        {
            //luu vaof session
            return (taikhoanadmin)HttpContext.Current.Session["user"];
        }
            
    }
}