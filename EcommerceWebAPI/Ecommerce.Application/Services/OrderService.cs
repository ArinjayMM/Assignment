using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EcommerceWebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            Log.Information($"Getting all orders => {orders}");
            return orders;
        }

        public async Task<IEnumerable<Orders>> SearchOrders(string searchTerm)
        {
            var order = await _orderRepository.SearchOrders(searchTerm);
            Log.Information($"Getting order for search {searchTerm} => {searchTerm}");
            return order;
        }

        public async Task<IEnumerable<Orders>> GetUserOrders(int userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);
            Log.Information($"Getting orders for user ID: {userId} => {orders}");
            return orders;
        }

        public async Task<Orders> PlaceOrder(Orders order)
        {
            // Generate order number
            order.OrderNo = GenerateOrderNumber();
            order.CreatedOn = DateTime.Now;
            var addOrder = await _orderRepository.PlaceOrder(order);
            Log.Information($"Order placed. => {addOrder}");
            return addOrder;
        }

        public async Task<Orders> UpdateOrderStatus(int id, string orderStatus)
        {
            var order = await _orderRepository.UpdateOrderStatus(id, orderStatus);
            Log.Information($"Updated order status for order ID: {id} => {orderStatus}");
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _orderRepository.DeleteOrder(id);
            Log.Information($"Deleted order with ID: {id} => {order}");
            return order;
        }

        private string GenerateOrderNumber()
        {
            // Generate a random 6-digit order number
            return "ORD" + (100000 + new Random().Next(900000)).ToString();
        }
    }
}
