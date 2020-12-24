using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Consts;
using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using MobilePhoneStoreDBMS.Models.ViewModels.Account;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace MobilePhoneStoreDBMS.Controllers
{
    public class AccountController : Controller
    {
        private MobilePhoneStoreDBMSEntities _context;
        public AccountController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        
        public ActionResult Login(int roleID = RoleIds.Unknown)
        {
            var loginViewModel = new LoginViewModel() { AccountDto = new AccountDto(), RoleID = roleID };

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View("Login", loginViewModel);
            }

            var acc = loginViewModel.AccountDto;

            var result = Login(acc.Username, acc.Password);

            if (result == 0) // Invalid user name or password
                return View(loginViewModel);

            var accInDb = this._context.Accounts.SingleOrDefault(a => a.Username == acc.Username);

            if (accInDb == null)
                throw new Exception("Not found");

            if (result == RoleIds.Admin)
            {
                Session[SessionNames.AdminID] = accInDb.AccID;
                return RedirectToAction("Index", "Admin");
            }
            else if (result == RoleIds.Seller)
            {
                Session[SessionNames.SellerID] = accInDb.AccID;
                return RedirectToAction("Index", "Seller");
            }
            else if (result == RoleIds.Customer)
            {
                Session[SessionNames.CustomerID] = accInDb.AccID;
                return RedirectToAction("Index", "HomeScreen");
            }
            
            return View(loginViewModel);
        }

        [HttpGet]
        public ActionResult Register()
        {
            var registerDto = new RegisterDto();
            return View(registerDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var result = Register(registerDto.Name, registerDto.PhoneNumber, registerDto.Email, registerDto.Username, registerDto.Password);
            return View(registerDto);
        }

        public ActionResult Logout(string sessionName)
        {
            if (Session[sessionName] == null)
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);

            Session[sessionName] = null;

            return RedirectToAction("Index", "HomeScreen");
        }

        private int Login(string username, string password)
        {
            string pwd = Syptop.Encrypt(password, true);
            SqlParameter[] sqlParams =
            {
                new SqlParameter("@username", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@password", System.Data.SqlDbType.NVarChar)
            };

            sqlParams[0].Value = username;
            sqlParams[1].Value = pwd;

            var res = this._context.Database.SqlQuery<int>("sp_Account_Login @username, @password", sqlParams).SingleOrDefault();

            return res;
        }
        private bool Register(string name, string phone, string email, string username, string password)
        {
            string pwd = Syptop.Encrypt(password, true);

            SqlParameter[] sqlParams =
            {
                new SqlParameter("@Name", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@PhoneNumber", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@Email", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@username", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@password", System.Data.SqlDbType.NVarChar)
            };

            sqlParams[0].Value = name;
            sqlParams[1].Value = phone;
            sqlParams[2].Value = email;
            sqlParams[3].Value = username;
            sqlParams[4].Value = pwd;

            var res = this._context.Database.SqlQuery<bool>("sp_Account_Register @Name, @PhoneNumber, @Email, @username, @password", sqlParams).SingleOrDefault();

            return res;
        }
    }
}