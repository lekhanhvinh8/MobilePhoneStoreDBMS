using MobilePhoneStoreDBMS.Models.Consts;
using MobilePhoneStoreDBMS.Models.Entities;
using MobilePhoneStoreDBMS.Models.ViewModels.Account;
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
            if (!CheckLoginForAdmin())
                return RedirectToAction("Login", "Account", new { roleID = RoleIds.Admin });

            return View(this._context);
        }

        private bool CheckLoginForAdmin()
        {
            if (Session[SessionNames.AdminID] == null)
                return false;

            return true;
        }
    }
}