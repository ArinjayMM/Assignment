using Microsoft.AspNetCore.Mvc;
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Services.Interfaces;
using Serilog;

namespace EcommerceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while fetching ordered items! => {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Orders>>> SearchOrders(string? searchTerm = null)
        {
            try
            {
                var orders = string.IsNullOrEmpty(searchTerm)
                    ? await _orderService.GetAllOrders()
                    : await _orderService.SearchOrders(searchTerm);

                return Ok(orders);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while searching orders! => {@ex}",ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetUserOrders(int userId)
        {
            try
            {
                var orderItems = await _orderService.GetUserOrders(userId);
                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while fetching orderd items! => {@ex}",ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Orders>> PlaceOrder(Orders order)
        {
            try
            {
                var placedOrder = await _orderService.PlaceOrder(order);
                return Ok(placedOrder);
            }
            catch (Exception ex)
            {
                // Log the exception for further analysis
                Log.Error("An error occured while placing the order! => {@ex}",ex);
                return BadRequest("An error occurred while placing the order.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Orders>> UpdateOrderStatus(int id, string orderStatus)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatus(id, orderStatus);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while updating order item! => {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrder(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while deleting order item! => {ex}",ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
