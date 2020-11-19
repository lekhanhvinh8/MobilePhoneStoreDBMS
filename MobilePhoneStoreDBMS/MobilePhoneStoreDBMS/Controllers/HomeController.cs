using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilePhoneStoreDBMS.Models.Entities;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class HomeController : Controller
    {
        private MobilePhoneStoreDBMSEntities _context;

        public HomeController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        public ActionResult Index()
        {
            return View(this._context);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}