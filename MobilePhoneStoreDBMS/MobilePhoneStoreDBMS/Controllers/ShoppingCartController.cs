using MobilePhoneStoreDBMS.Models;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilePhoneStoreDBMS.Models.Consts;

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
                return RedirectToAction("Login", "Account");
            }

            int userid = Convert.ToInt32(Session[SessionNames.CustomerID]);

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
        //
        public PartialViewResult BagCart()
        {
            //int _t_item = 0;
            //Cart_ cart = Session["cart"] as Cart_;
            //if (cart != null)
            //{
            //    _t_item = cart.Total_Quantity();
            //}
            //ViewBag.infoCart = _t_item;
            return PartialView("BagCart");
        }
        //

    }
}