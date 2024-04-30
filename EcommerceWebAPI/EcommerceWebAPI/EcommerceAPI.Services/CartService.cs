using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<Cart>> GetAllCartItems()
        {
            return await _cartRepository.GetAllCartItems();
        }

        public async Task<IEnumerable<Cart>> GetCartItems(int userId)
        {
            return await _cartRepository.GetCartItems(userId);
        }

        public async Task<Cart> AddtoCart(Cart cart)
        {
            return await _cartRepository.AddtoCart(cart);
        }

        public async Task<Cart> UpdateCartItem(int id, Cart cart)
        {
            return await _cartRepository.UpdateCartItem(id, cart);
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            return await _cartRepository.DeleteCartItem(id);
        }
    }
}
