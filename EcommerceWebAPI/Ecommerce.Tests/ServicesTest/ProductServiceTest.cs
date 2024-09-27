using NUnit.Framework;
using EcommerceWebAPI.Services;
using EcommerceWebAPI.Models;
using Moq;
using Microsoft.Extensions.Logging;
using EcommerceWebAPI.Repositories.Interfaces;

namespace Ecommerce.Tests.ServicesTest
{
    [TestFixture]
    public class ProductServiceTest
    {
        private ProductService _productService;
        private Mock<IProductRepository> _productRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_productRepositoryMock.Object, loggerMock.Object);
        }

        [Test]
        public async Task TestGetProducts()
        {
            // Arrange
            var expectedProducts = new List<Products> {
                new Products { ID = 1, Name = "Product 1", UnitPrice = 10.0m, Discount = 0, ImageUrl = "image1.jpg", Status = 0, MRP = 10.99m },
                new Products { ID = 2, Name = "Product 2", UnitPrice = 15.0m, Discount = 0, ImageUrl = "image2.jpg", Status = 0, MRP = 20.99m }
            };
            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProducts.Count, result.Count());
        }

        [Test]
        public async Task TestAddProduct()
        {
            // Arrange
            var newProduct = new Products { Name = "New Product", Manufacturer = "Manufacturer", UnitPrice = 20.0m, Discount = 0, ImageUrl = "new_product.jpg", Status = 0, MRP = 20.99m };

            _productRepositoryMock.Setup(repo => repo.AddProduct(It.IsAny<Products>())).ReturnsAsync(newProduct);

            // Act
            var result = await _productService.AddProduct(newProduct);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newProduct.Name, result.Name);
        }

        [Test]
        public async Task TestUpdateProduct()
        {
            // Arrange
            int productId = 1;
            var updatedProduct = new Products { ID = productId, Name = "Updated Product", Manufacturer = "Manufacturer", UnitPrice = 25.0m, Discount = 0, ImageUrl = "updated_product.jpg", Status = 0, MRP = 25.99m };

            _productRepositoryMock.Setup(repo => repo.UpdateProduct(productId, It.IsAny<Products>())).ReturnsAsync(updatedProduct);

            // Act
            var result = await _productService.UpdateProduct(productId, updatedProduct);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedProduct.Name, result.Name);
        }


        [Test]
        public async Task TestDeleteProduct()
        {
            // Arrange
            int productId = 2;

            _productRepositoryMock.Setup(repo => repo.DeleteProduct(productId)).ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
