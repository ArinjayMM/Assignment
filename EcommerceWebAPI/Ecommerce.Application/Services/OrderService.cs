using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Getting all orders...");
            return await _orderRepository.GetAllOrders();
        }

        public async Task<IEnumerable<Orders>> GetUserOrders(int userId)
        {
            _logger.LogInformation($"Getting orders for user ID: {userId}");
            return await _orderRepository.GetUserOrders(userId);
        }

        public async Task<Orders> PlaceOrder(Orders order)
        {
            _logger.LogInformation("Placing order...");
            // Generate order number
            order.OrderNo = GenerateOrderNumber();
            order.CreatedOn = DateTime.Now;
            return await _orderRepository.PlaceOrder(order);
        }

        public async Task<Orders> UpdateOrderStatus(int id, string orderStatus)
        {
            _logger.LogInformation($"Updating order status for order ID: {id}");
            return await _orderRepository.UpdateOrderStatus(id, orderStatus);
        }

        public async Task<bool> DeleteOrder(int id)
        {
            _logger.LogInformation($"Deleting order with ID: {id}");
            return await _orderRepository.DeleteOrder(id);
        }

        private string GenerateOrderNumber()
        {
            // Generate a random 6-digit order number
            return "ORD" + (100000 + new Random().Next(900000)).ToString();
        }
    }
}
