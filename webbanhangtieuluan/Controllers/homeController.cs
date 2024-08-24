using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webbanhangtieuluan.Models;
namespace webbanhangtieuluan.Controllers
{
    public class homeController : Controller
    {
        QLmyphamdbmlDataContext db = new QLmyphamdbmlDataContext();
        //
        // GET: /home/
        public ActionResult Index()
        {
            return View(db.SanPhams.OrderBy(s=>s.GiaBan).Take(10).ToList());
        }
        public ActionResult Index1()
        {
            return View(db.SanPhams.OrderBy(s => s.GiaBan).Take(10).ToList());
        }
	}
}