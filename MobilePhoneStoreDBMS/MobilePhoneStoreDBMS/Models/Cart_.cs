using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models
{
    public class CartItem
    {
        public Product _shopping_product { get; set; }
        public int _shopping_quantity { get; set; }
    }
    public class Cart_
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public int Total_Quantity()
        {
            return items.Sum(s => s._shopping_quantity);
        }
    }
}