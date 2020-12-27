using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilePhoneStoreDBMS.Models.Consts;
using System.Web.Http;
using System.Net;
namespace MobilePhoneStoreDBMS.Controllers
{
    public class ShoppingCartController : Controller
    {
        MobilePhoneStoreDBMSEntities _db = new MobilePhoneStoreDBMSEntities();
        // GET: ShoppingCart
        // add item vao gio hang
        public ActionResult AddtoCart(int id)
        {
            if (Session[SessionNames.CustomerID] == null)
            {
                return RedirectToAction("Login", "Account", new { RoleId = RoleIds.Customer });
            }

            int userid = Convert.ToInt32(Session[SessionNames.CustomerID]);

            var product = this._db.Products.Find(id);

            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (product.Quantity == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            Cart c = _db.Carts.Where(x => x.CustomerID == userid && x.ProductID == id).SingleOrDefault();

            if (c == null)
            {
                Cart cart = new Cart();
                cart.ProductID = id;
                cart.CustomerID = userid;
                cart.amount = 1;
                _db.Carts.Add(cart);
                _db.SaveChanges();
            }
            else
            {
                c.amount += 1;
                _db.Carts.AddOrUpdate(c);
                _db.SaveChanges();
            }

            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        // trang gio hang
        public ActionResult ShowToCart()
        {
            int userid = Convert.ToInt32(Session[SessionNames.CustomerID]);
            List<Cart> cart = _db.Carts.Where(x => x.CustomerID == userid).ToList();
            return View(cart);
        }
        //
        public ActionResult Update_Quantity_Cart(int ID_Product, int quantity)
        {
            int userid = Convert.ToInt32(Session[SessionNames.CustomerID]);
            Cart cart = _db.Carts.Where(x => x.CustomerID == userid && x.ProductID == ID_Product).SingleOrDefault();

            var product = this._db.Products.Find(ID_Product);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (product.Quantity < quantity - cart.amount)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            cart.amount = quantity;
            _db.Carts.AddOrUpdate(cart);
            _db.SaveChanges();
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        //
        public ActionResult RemoveCart(int id)
        {
            int userid = Convert.ToInt32(Session[SessionNames.CustomerID]);
            Cart cart = _db.Carts.Where(x => x.CustomerID == userid && x.ProductID == id).SingleOrDefault();
            _db.Carts.Remove(cart);
            _db.SaveChanges();
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        public ActionResult RemoveAllCart()
        {
            int userid = Convert.ToInt32(Session[SessionNames.CustomerID]);
            List<Cart> cart = _db.Carts.Where(x => x.CustomerID == userid).ToList();
            foreach (var i in cart)
            {
                _db.Carts.Remove(i);
                
            }
            _db.SaveChanges();

            return RedirectToAction("ShowToCart", "ShoppingCart");
        }

        public ActionResult CreateAnOrder()
        {
            int customerID = Convert.ToInt32(Session[SessionNames.CustomerID]);

            var carts = this._db.Carts.Where(c => c.CustomerID == customerID).ToList();

            if (carts.Count() == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            foreach (var cart in carts)
            {
                if (cart.amount > cart.Product.Quantity)
                    throw new Exception("Invalid amount of products inserting to orders");
            }

            int noOfRowEffected = this._db.Database.ExecuteSqlCommand("Insert into Orders(CustomerID) Values(" + customerID + ")");

            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        //
        public PartialViewResult BagCart()
        {
            if (Session[SessionNames.CustomerID] != null)
            {
                int userid = Convert.ToInt32(Session[SessionNames.CustomerID]);
                var amount = (from i in _db.Carts
                              where i.CustomerID == userid
                              select (int?)i.amount).Sum() ?? 0;
                ViewBag.infoCart = amount;
            }
            return PartialView("BagCart");
        }

    }
}