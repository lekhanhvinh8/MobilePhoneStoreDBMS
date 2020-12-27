using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreDBMS.Controllers
{
    public class HomeScreenController : Controller
    {
        // GET: HomeScreen
        private MobilePhoneStoreDBMSEntities _context;

        public HomeScreenController()
        {
            _context = new MobilePhoneStoreDBMSEntities();
        }
        //GET: HomeScreen/
        public ActionResult Index()
        {
            List<Product> allProducts = _context.Database.SqlQuery<Product>("Sp_Product_List").ToList();
            return View(allProducts);
        }

        public ActionResult Store()
        {
            var viewModel = new ViewModel();

            viewModel.allProducts = _context.Database.SqlQuery<Product>("Sp_Product_List").ToList();
            viewModel.allProducers = _context.Database.SqlQuery<Producer>("Sp_Producer_List").ToList();
           
            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var product = _context.Database.SqlQuery<Product>("Sp_Product_Details @id = {0}", id).Single();

            ViewBag.ProId = id;
            return View(product);
        }

        public ActionResult Producer(int id)
        {
            var model = new ViewModel();
            model.producer = _context.Database.SqlQuery<Producer>("Sp_Producer_Details @id = {0}", id).Single();
            model.allProductsOfProducer = _context.Database.SqlQuery<Product>("Sp_Product_List_Of_Producer @producerID = {0}", id).ToList();
            return View(model);
        }

        public ActionResult Category(int id)
        {
            var model = new ViewModel();
            model.category = _context.Database.SqlQuery<Category>("Sp_Catagory_Details @id = {0}", id).Single();
            model.allProductsOfCategory = _context.Database.SqlQuery<Product>("Sp_Product_List_Of_Category @categoryID = {0}", id).ToList();
            model.allProducers = _context.Database.SqlQuery<Producer>("Sp_Producer_List").ToList();

            return View(model);
        }


        public ActionResult ProductsOfSearch(string search = "")
        {
            ViewBag.search = search;
            if (search == "" || search == null)
            {
                var model = new ViewModel();
                model.productsOfSearch = (from i in _context.Products
                                          where i.Name.Contains(search)
                                          where i.Status == true
                                          select i).OrderBy(p => p.Name).ToList();
                model.allProducers = _context.Database.SqlQuery<Producer>("Sp_Producer_List").ToList();
                return View(model);
            }
            else
            {
                var model = new ViewModel();
                model.productsOfSearch = (from i in _context.Products
                                          where i.Name.Contains(search)
                                          where i.Status == true
                                          select i).OrderBy(p => p.Name).ToList();
                model.allProducers = _context.Database.SqlQuery<Producer>("Sp_Producer_List").ToList();
                return View(model);
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();

            return RedirectToAction("Index");
        }
    }
}