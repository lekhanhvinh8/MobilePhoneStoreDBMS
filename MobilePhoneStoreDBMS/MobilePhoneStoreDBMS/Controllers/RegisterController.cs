using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Entities;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(String Name, String PhoneNumber, String Email, string confirm, Account a)
        {
            try
            {
                if (confirm != a.Password)
                {
                    ViewBag.msg = "Password is not match!";
                }
                else
                {
                    var result = new RegisterModel().Register(Name, PhoneNumber, Email, a.Username, a.Password);

                    if (result)
                    {
                        ViewBag.msg = "Sucess!";
                    }
                    else
                    {
                        ViewBag.msg = "Error!";
                    }
                }
            }
            catch
            {
                ViewBag.msg = "Register Fail!";
            }

            return View();
        }
    }
}