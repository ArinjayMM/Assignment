using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<IEnumerable<Orders>> GetUserOrders(int userId)
        {
            return await _orderRepository.GetUserOrders(userId);
        }

        public async Task<Orders> PlaceOrder(Orders order)
        {
            // Generate order number
            order.OrderNo = GenerateOrderNumber();
            order.CreatedOn = DateTime.Now;
            return await _orderRepository.PlaceOrder(order);
        }

        public async Task<Orders> UpdateOrderStatus(int id, string orderStatus)
        {
            return await _orderRepository.UpdateOrderStatus(id, orderStatus);
        }

        public async Task<bool> DeleteOrder(int id)
        {
            return await _orderRepository.DeleteOrder(id);
        }

        private string GenerateOrderNumber()
        {
            // Generate a random 6-digit order number
            return "ORD" + (100000 + new Random().Next(900000)).ToString();
        }
    }
}
