﻿using System;
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
        MobilePhoneStoreDBMSEntities _db = new MobilePhoneStoreDBMSEntities();

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
                    int acc_id = _db.Accounts.Where(x => x.Username == acc.Username).SingleOrDefault().AccID;
                    int user_id = _db.Customers.Where(x => x.hasAcc == acc_id).SingleOrDefault().CustomerID;

                    Session["user_id"] = user_id;
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