// IProductRepository.cs
using EcommerceWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllProducts();
        Task<Products> GetProductById(int id);
        Task<Products> AddProduct(Products product);
        Task<Products> UpdateProduct(int id, Products product);
        Task<bool> DeleteProduct(int id);
    }
}
