using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EcommerceWebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            _logger.LogInformation("Getting all products...");
            return await _productRepository.GetAllProducts();
        }

        public async Task<Products> GetProductById(int id)
        {
            _logger.LogInformation($"Getting product with ID: {id}");
            return await _productRepository.GetProductById(id);
        }

        public async Task<Products> AddProduct(Products product)
        {
            _logger.LogInformation("Adding product...");
            return await _productRepository.AddProduct(product);
        }

        public async Task<Products> UpdateProduct(int id, Products product)
        {
            _logger.LogInformation($"Updating product with ID: {id}");
            return await _productRepository.UpdateProduct(id, product);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            _logger.LogInformation($"Deleting product with ID: {id}");
            return await _productRepository.DeleteProduct(id);
        }
    }
}
