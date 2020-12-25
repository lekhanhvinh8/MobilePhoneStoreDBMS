using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Consts;
using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.ViewModels.Account;
using MobilePhoneStoreDBMS.Models.ViewModels.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller
        public ActionResult Index()
        {
            if(!CheckLoginForSeller())
                return RedirectToAction("Login", "Account", new LoginViewModel() { AccountDto = new AccountDto(), RoleID = RoleIds.Seller});
            return View();
        }
        public ActionResult AddNewProduct()
        {
            var productForSellerViewModel = new ProductForSellerViewModel();

            productForSellerViewModel.Mode = "Add";

            if(!CheckLoginForSeller())
            {
                productForSellerViewModel.IsRoleSellerLogin = false;
                return RedirectToAction("Login", "Account", new LoginViewModel() { AccountDto = new AccountDto(), RoleID = RoleIds.Seller });
            }

            productForSellerViewModel.IsRoleSellerLogin = true;
            productForSellerViewModel.SellerID = (int)Session[SessionNames.SellerID];

            return View(productForSellerViewModel);
        }

        private bool CheckLoginForSeller()
        {
            if (Session[SessionNames.SellerID] == null)
            {
                return false;
            }
            return true;
        }
    }
}