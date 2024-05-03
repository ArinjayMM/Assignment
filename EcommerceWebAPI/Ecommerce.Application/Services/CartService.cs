using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Getting all cart items...");
            return await _cartRepository.GetAllCartItems();
        }

        public async Task<IEnumerable<Cart>> GetCartItems(int userId)
        {
            _logger.LogInformation($"Getting cart items for user ID: {userId}");
            return await _cartRepository.GetCartItems(userId);
        }

        public async Task<Cart> AddtoCart(Cart cart)
        {
            _logger.LogInformation("Adding to cart...");
            return await _cartRepository.AddtoCart(cart);
        }

        public async Task<Cart> UpdateCartItem(int id, Cart cart)
        {
            _logger.LogInformation($"Updating cart item with ID: {id}");
            return await _cartRepository.UpdateCartItem(id, cart);
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            _logger.LogInformation($"Deleting cart item with ID: {id}");
            return await _cartRepository.DeleteCartItem(id);
        }
    }
}
