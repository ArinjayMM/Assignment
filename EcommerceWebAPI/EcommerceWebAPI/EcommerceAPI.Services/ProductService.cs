// ProductService.cs
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<Products> AddProduct(Products product)
        {
            return await _productRepository.AddProduct(product);
        }

        public async Task<Products> UpdateProduct(int id, Products product)
        {
            return await _productRepository.UpdateProduct(id, product);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProduct(id);
        }
    }
}
