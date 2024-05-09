using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EcommerceWebAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Cart>> GetAllCartItems()
        {
            var items = await _cartRepository.GetAllCartItems();
            Log.Information("All cart items => {@items}", items);
            return items;
        }

        public async Task<IEnumerable<Cart>> GetCartItems(int userId)
        {
            var item = await _cartRepository.GetCartItems(userId);
            Log.Error($"Getting cart items for user ID: {userId} => {item}");
            return item;
        }

        public async Task<Cart> AddtoCart(Cart cart)
        {
            var item = await _cartRepository.AddtoCart(cart);
            Log.Error($"Adding cart item = > {item}");
            return item;
        }

        public async Task<Cart> UpdateCartItem(int id, Cart cart)
        {
            var item = await _cartRepository.UpdateCartItem(id, cart);
            Log.Error($"Updating cart item with ID: {id} => {item}");
            return item;
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            var item = await _cartRepository.DeleteCartItem(id);
            Log.Error($"Deleting cart item with ID: {id} => {item}");
            return item;
        }
    }
}
