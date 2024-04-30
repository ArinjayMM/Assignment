using Microsoft.AspNetCore.Mvc;
using EcommerceWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceWebAPI.Services.Interfaces;

namespace EcommerceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
                Console.WriteLine($"Error placing order: {ex}");
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
                return BadRequest(ex.Message);
            }
        }
    }
}
