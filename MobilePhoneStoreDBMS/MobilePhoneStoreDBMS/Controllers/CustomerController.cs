using MobilePhoneStoreDBMS.Models.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index(int customerID)
        {
            CustomerViewModel customerViewModel = new CustomerViewModel() { customerID = customerID };
            return View(customerViewModel);
        }
    }
}