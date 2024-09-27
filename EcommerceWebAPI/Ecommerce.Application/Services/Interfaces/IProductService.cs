﻿// IProductService.cs
using EcommerceWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> GetAllProducts();
        Task<IEnumerable<Products>> SearchProducts(string searchTerm);
        Task<Products> GetProductById(int id);
        Task<Products> AddProduct(Products product);
        Task<Products> UpdateProduct(int id, Products product);
        Task<bool> DeleteProduct(int id);
    }
}
