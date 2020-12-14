using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Entities;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Account acc)
        {
            try
            {
                var result = new LoginModel().Login(acc.Username, acc.Password);

                if (result == 1)
                {
                    Session["admin_id"] = acc.AccID;
                    return RedirectToAction("Index", "Admin");
                }
                else if (result == 2)
                {
                    Session["user_id"] = acc.AccID;
                    return RedirectToAction("Index", "HomeScreen");
                }
                else if (result == 3)
                {
                    Session["emp_id"] = acc.AccID;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.msg = "Email or Password Isvalid!";
                }
            }
            catch
            {
                
            }

            return View();
        }
    }
}