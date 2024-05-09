using NUnit.Framework;
using EcommerceWebAPI.Services;
using EcommerceWebAPI.Models;
using Moq;
using Microsoft.Extensions.Logging;
using EcommerceWebAPI.Repositories.Interfaces;

namespace Ecommerce.Tests.ServicesTest
{
    [TestFixture]
    public class CartServiceTest
    {
        private CartService _cartService;
        private Mock<ICartRepository> _cartRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            var loggerMock = new Mock<ILogger<CartService>>();
            _cartService = new CartService(_cartRepositoryMock.Object, loggerMock.Object);
        }

        [Test]
        public async Task TestGetAllCartItems()
        {
            // Arrange
            var expectedCartItems = new List<Cart> {
                new Cart { ID = 1, UserId = 1, ProductID = 1, Quantity = 2, TotalPrice = 20.0m },
                new Cart { ID = 2, UserId = 1, ProductID = 2, Quantity = 1, TotalPrice = 15.0m }
            };
            _cartRepositoryMock.Setup(repo => repo.GetAllCartItems()).ReturnsAsync(expectedCartItems);

            // Act
            var result = await _cartService.GetAllCartItems();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCartItems.Count, result.Count());
        }

        [Test]
        public async Task TestGetCartItems()
        {
            // Arrange
            int userId = 1;
            var expectedCartItems = new List<Cart> {
                new Cart { ID = 1, UserId = userId, ProductID = 1, Quantity = 2, TotalPrice = 20.0m },
                new Cart { ID = 2, UserId = userId, ProductID = 2, Quantity = 1, TotalPrice = 15.0m }
            };
            _cartRepositoryMock.Setup(repo => repo.GetCartItems(userId)).ReturnsAsync(expectedCartItems);

            // Act
            var result = await _cartService.GetCartItems(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCartItems.Count, result.Count());
        }

        [Test]
        public async Task TestAddtoCart()
        {
            // Arrange
            var newCartItem = new Cart { UserId = 1, ProductID = 3, Quantity = 1 };
            _cartRepositoryMock.Setup(repo => repo.AddtoCart(It.IsAny<Cart>())).ReturnsAsync(newCartItem);

            // Act
            var result = await _cartService.AddtoCart(newCartItem);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newCartItem.UserId, result.UserId);
            Assert.AreEqual(newCartItem.ProductID, result.ProductID);
            Assert.AreEqual(newCartItem.Quantity, result.Quantity);
        }

        [Test]
        public async Task TestUpdateCartItem()
        {
            // Arrange
            int cartItemId = 1;
            var updatedCartItem = new Cart { ID = cartItemId, Quantity = 3 };

            _cartRepositoryMock.Setup(repo => repo.UpdateCartItem(cartItemId, It.IsAny<Cart>())).ReturnsAsync(updatedCartItem);

            // Act
            var result = await _cartService.UpdateCartItem(cartItemId, updatedCartItem);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedCartItem.ID, result.ID);
            Assert.AreEqual(updatedCartItem.Quantity, result.Quantity);
        }

        [Test]
        public async Task TestDeleteCartItem()
        {
            // Arrange
            int cartItemId = 2;

            _cartRepositoryMock.Setup(repo => repo.DeleteCartItem(cartItemId)).ReturnsAsync(true);

            // Act
            var result = await _cartService.DeleteCartItem(cartItemId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
