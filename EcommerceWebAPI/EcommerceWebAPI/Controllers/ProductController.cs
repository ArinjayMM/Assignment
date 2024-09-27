using EcommerceWebAPI.Models;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EcommerceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured while fetching products! => {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Products>>> SearchProducts(string? searchTerm = null)
        {
            try
            {
                var products = string.IsNullOrEmpty(searchTerm)
                    ? await _productService.GetAllProducts()
                    : await _productService.SearchProducts(searchTerm);

                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while searching product! => {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound("Product not found with entered id!");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured while fetching product details! => {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Products>> AddProduct(Products product)
        {
            try
            {
                var newProduct = await _productService.AddProduct(product);
                return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ID }, newProduct);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured while adding product! => {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Products>> UpdateProduct(int id, Products product)
        {
            try
            {
                if (id != product.ID)
                {
                    return BadRequest($"Product with ID {id} does not match the ID in the provided product!");
                }
                var updatedProduct = await _productService.UpdateProduct(id, product);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured while updating product details! => {ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProduct(id);
                if (result)
                {
                    return Ok("Product deleted successfully!");
                }
                return NotFound("Product not found with entered id!");
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured while deleting product! => {ex}");
                return BadRequest(ex.Message);
            }
        }
    }
}
