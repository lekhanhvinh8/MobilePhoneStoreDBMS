using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Consts;
using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using MobilePhoneStoreDBMS.Models.ViewModels.Account;
using MobilePhoneStoreDBMS.Models.ViewModels.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class SellerController : Controller
    {
        private MobilePhoneStoreDBMSEntities _context;
        public SellerController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }

        // GET: Seller
        public ActionResult Index()
        {
            if(!CheckLoginForSeller())
                return RedirectToAction("Login", "Account", new LoginViewModel() { AccountDto = new AccountDto(), RoleID = RoleIds.Seller});
            return View();
        }
        public ActionResult AddNewProduct()
        {
            if(!CheckLoginForSeller())
            {
                return RedirectToAction("Login", "Account", new LoginViewModel() { AccountDto = new AccountDto(), RoleID = RoleIds.Seller });
            }

            return View();
        }

        public ActionResult UpdateProduct(int productID)
        {
            var product = this._context.Products.Find(productID);

            if (product == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            var productForSellerViewModel = new ProductForSellerViewModel();

            productForSellerViewModel.productID = product.ProductID;

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