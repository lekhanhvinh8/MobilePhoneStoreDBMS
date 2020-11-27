using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private MobilePhoneStoreDBMSEntities _context;

        public AdminController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        public ActionResult Index()
        {
            return View(this._context);
        }
    }
}