using Dapper;
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            using IDbConnection dbConnection = _context.CreateConnection();
            return await dbConnection.QueryAsync<Cart>("cartItems", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Cart>> GetCartItems(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new { UserId = userId };
                var cartItems = await connection.QueryAsync<Cart, Products, Cart>(
                    "GetCartItemsByUserId",
                    (cart, product) =>
                    {
                        cart.products = product;
                        return cart;
                    },
                    parameters,
                    splitOn: "ID",
                    commandType: CommandType.StoredProcedure);

                return cartItems;
            }
        }

        public async Task<Cart> AddtoCart(Cart cart)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new
            {
                cart.UserId,
                cart.ProductID,
                cart.UnitPrice,
                cart.Discount,
                cart.Quantity,
                cart.TotalPrice
            };
            await dbConnection.ExecuteAsync("addCartItem", parameters, commandType: CommandType.StoredProcedure);
            return cart;
        }

        public async Task<Cart> UpdateCartItem(int id, Cart cart)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { ID = id, Quantity = cart.Quantity };
            await dbConnection.ExecuteAsync("updateCartItem", parameters, commandType: CommandType.StoredProcedure);
            return cart;
        }

        public async Task<bool> DeleteCartItem(int id)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Id = id };
            await dbConnection.ExecuteAsync("deleteCartItem", parameters, commandType: CommandType.StoredProcedure);
            return true;
        }
    }
}
