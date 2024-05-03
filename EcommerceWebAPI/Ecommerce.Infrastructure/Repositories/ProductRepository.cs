using Dapper;
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EcommerceWebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperDBContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DapperDBContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            return await dbConnection.QueryAsync<Products>("allProducts", commandType: CommandType.StoredProcedure);
        }

        public async Task<Products> GetProductById(int id)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Id = id };
            return await dbConnection.QueryFirstOrDefaultAsync<Products>("productById", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<Products> AddProduct(Products product)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new
            {
                product.Name,
                product.Manufacturer,
                product.MRP,
                product.Discount,
                product.ImageUrl,
                product.Status
            };
            await dbConnection.ExecuteAsync("addProduct", parameters, commandType: CommandType.StoredProcedure);
            return product;
        }

        public async Task<Products> UpdateProduct(int id, Products product)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new
            {
                product.ID,
                product.Name,
                product.Manufacturer,
                product.MRP,
                product.Discount,
                product.ImageUrl,
                product.Status
            };
            await dbConnection.ExecuteAsync("updateProduct", parameters, commandType: CommandType.StoredProcedure);
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Id = id };
            await dbConnection.ExecuteAsync("deleteProduct", parameters, commandType: CommandType.StoredProcedure);
            return true;
        }
    }
}
