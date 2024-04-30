using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DapperDBContext _context;

        public CartRepository(DapperDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllCartItems()
        {
            return await _context.Cart.ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartItems(int userId)
        {
            return await _context.Cart
                .Where(c => c.UserId == userId)
                .Include(c => c.products)
                .ToListAsync();
        }

        public async Task<Cart> AddtoCart(Cart cart)
        {
            var product = await _context.Products.FindAsync(cart.ProductID);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            cart.UnitPrice = product.UnitPrice;
            cart.Discount = product.Discount;
            cart.Quantity = 1;
            cart.TotalPrice = product.UnitPrice * cart.Quantity;

            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> UpdateCartItem(int id, Cart cart)
        {
            if (id != cart.ID)
            {
                throw new Exception($"Entered id {id} doesn't match with {cart.ID}");
            }

            var existingCartItem = await _context.Cart.FindAsync(id);

            if (existingCartItem == null)
            {
                throw new Exception("Cart item not found");
            }

            existingCartItem.Quantity = cart.Quantity;
            existingCartItem.TotalPrice = existingCartItem.UnitPrice * cart.Quantity;

            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            var cartItem = await _context.Cart.FindAsync(id);

            if (cartItem == null)
            {
                return false;
            }

            _context.Cart.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
