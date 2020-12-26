using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class ProductsOfOrderDto
    {
        public ProductsOfOrderDto()
        {
        }

        public ProductsOfOrderDto(ProductsOfOrder productsOfOrder)
        {
            this.OrderID = productsOfOrder.OrderID;
            this.ProductID = productsOfOrder.ProductID;
            this.amount = productsOfOrder.amount;
        }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> amount { get; set; }

    }
}