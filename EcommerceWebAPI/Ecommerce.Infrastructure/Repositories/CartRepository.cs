using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;

namespace EcommerceWebAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DapperDBContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(DapperDBContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            // Get product details to calculate total price
            var product = await dbConnection.QueryFirstOrDefaultAsync<Products>(
                "SELECT * FROM Products WHERE ID = @ProductId",
                new { ProductId = cart.ProductID });

            // Calculate total price based on quantity, unit price, and discount
            cart.Quantity = 1;
            cart.UnitPrice = product.UnitPrice;
            cart.Discount = product.Discount;
            cart.TotalPrice = cart.Quantity * cart.UnitPrice;

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
