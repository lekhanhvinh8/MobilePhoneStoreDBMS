using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobilePhoneStoreDBMS.Models.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
            this.ProductOfOrderDtos = new List<ProductsOfOrderDto>();
        }
        public OrderDto(Order order)
        {
            this.CustomerID = order.CustomerID;
            this.OrderTime = order.OrderTime;
            this.Status = order.status;

            this.ProductOfOrderDtos = new List<ProductsOfOrderDto>();
            foreach (var product in order.ProductsOfOrders)
            {
                this.ProductOfOrderDtos.Add(new ProductsOfOrderDto(product));
            }
        }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public System.DateTime OrderTime { get; set; }
        public Nullable<int> Status { get; set; }
        public List<ProductsOfOrderDto> ProductOfOrderDtos { get; set; }

    }
}