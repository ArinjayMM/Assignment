// Repositories/ProductRepository.cs
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperDBContext _context;

        public ProductRepository(DapperDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Products> AddProduct(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Products> UpdateProduct(int id, Products product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
