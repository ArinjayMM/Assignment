using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

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
            var products = await _productRepository.GetAllProducts();
            Log.Information($"Getting all products {products}");
            return products;
        }

        public async Task<IEnumerable<Products>> SearchProducts(string searchTerm)
        {
            var products = await _productRepository.SearchProducts(searchTerm);
            Log.Information($"Getting product with search term {searchTerm} => {products}");
            return  products;
        }

        public async Task<Products> GetProductById(int id)
        {
            Log.Information($"Getting product with ID: {id}");
            return await _productRepository.GetProductById(id);
        }

        public async Task<Products> AddProduct(Products product)
        {
            var products = await _productRepository.AddProduct(product);
            Log.Information($"Product added => {products}");
            return products;
        }

        public async Task<Products> UpdateProduct(int id, Products product)
        {
            var updatedProduct = await _productRepository.UpdateProduct(id, product);
            Log.Information($"Updating product with ID: {id} => {updatedProduct}");
            return updatedProduct;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.DeleteProduct(id);
            Log.Information($"Deleting product with ID: {id} => {product}");
            return product;
        }
    }
}
