using Microsoft.AspNetCore.Mvc;
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Services.Interfaces;
using Serilog;

namespace EcommerceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCartItems()
        {
            try
            {
                var cartItems = await _cartService.GetAllCartItems();
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while fetching cart items! => {@ex}",ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartItems(int userId)
        {
            try
            {
                var cartItems = await _cartService.GetCartItems(userId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while fetching cart items! => {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> AddtoCart(Cart cart)
        {
            try
            {
                var addedCart = await _cartService.AddtoCart(cart);
                return Ok(addedCart);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while adding cart item => {@ex}",ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Cart>> UpdateCartItem(int id, Cart cart)
        {
            try
            {
                var updatedCart = await _cartService.UpdateCartItem(id, cart);
                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while updating cart item! => {@ex}",ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCartItem(int id)
        {
            try
            {
                var result = await _cartService.DeleteCartItem(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while deleting cart item! => {@ex}",ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
