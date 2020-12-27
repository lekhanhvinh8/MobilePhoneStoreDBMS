using MobilePhoneStoreDBMS.Models.Consts;
using MobilePhoneStoreDBMS.Models.Dtos;
using MobilePhoneStoreDBMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
        [HttpGet]
        public List<OrderDto> GetListByStatus(int status)
        {
            var orders = this._context.Orders.Where(o => o.status == status).ToList();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto(order));
            }

            return orderDtos;
        }

        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
        public OrderDto Get(int orderID)
        {
            var order = this._context.Orders.Find(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new OrderDto(order);
        }
        [HttpGet]
        public void ConfirmOrder(int orderID)
        {
            var order = this._context.Orders.Find(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (order.status != OrderStates.Pending)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            order.status = OrderStates.Confirmed;

            this._context.SaveChanges();
        }

        [HttpGet]
        public void CancelOrder(int orderID)
        {
            var order = this._context.Orders.Find(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (order.status != OrderStates.Pending)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));

            this._context.CancelAnOrder(orderID, OrderStates.Canceled, isSuccess);

            if (!(bool)isSuccess.Value)
                throw new Exception("Failure canceling an order");
        }
        [HttpGet]
        public void DeleteOrder(int orderID)
        {
            var order = this._context.Orders.Find(orderID);

            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (order.status != OrderStates.Canceled)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var isSuccess = new ObjectParameter("isSuccess", typeof(bool));

            this._context.DeleteAnOrder(orderID, OrderStates.Canceled, isSuccess);

            if (!(bool)isSuccess.Value)
                throw new Exception("Failure deleting an order");
        }
    }
}
