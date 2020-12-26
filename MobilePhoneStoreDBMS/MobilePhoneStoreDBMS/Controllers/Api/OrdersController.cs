using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobilePhoneStoreDBMS.Controllers.Api
{
    public class OrdersController : ApiController
    {
        MobilePhoneStoreDBMSEntities _context;

        public OrdersController()
        {
            this._context = new MobilePhoneStoreDBMSEntities();
        }
        public List<OrderDto> GetList(int customerID)
        {
            var orders = this._context.Orders.Where(o => o.CustomerID == customerID).ToList();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
        }

        public List<OrderDto> GetList(int customerID, DateTime orderTime)
        {
            var orders = this._context.Orders.Where(o => o.CustomerID == customerID && o.OrderTime == orderTime).ToList();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
        }
    }
}
