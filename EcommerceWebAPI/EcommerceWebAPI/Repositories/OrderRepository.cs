using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperDBContext _context;

        public OrderRepository(DapperDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Orders>> GetUserOrders(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.products)
                .ToListAsync();
        }

        public async Task<Orders> PlaceOrder(Orders order)
        {
            var product = await _context.Products.FindAsync(order.ProductID);
            if (product == null)
            {
                throw new Exception($"Product not found for ID: {order.ProductID}");
            }

            order.TotalPrice = product.UnitPrice * order.Quantity;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Orders> UpdateOrderStatus(int id, string orderStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new Exception($"Order not found for ID: {id}");
            }

            order.OrderStatus = orderStatus;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
