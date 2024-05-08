using EcommerceWebAPI.Services;
using EcommerceWebAPI.Models;
using Moq;
using Microsoft.Extensions.Logging;
using EcommerceWebAPI.Repositories.Interfaces;

namespace ECommerceApp.Tests
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private OrderService _orderService;
        private Mock<IOrderRepository> _orderRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            var loggerMock = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(_orderRepositoryMock.Object, loggerMock.Object);
        }

        [Test]
        public async Task TestGetAllOrders()
        {
            // Arrange
            var expectedOrders = new List<Orders> {
                new Orders { ID = 1, UserId = 1, OrderNo = "ORD123", ProductID = 1, Quantity = 2, OrderStatus = "Placed", CreatedOn = DateTime.Now },
                new Orders { ID = 2, UserId = 1, OrderNo = "ORD124", ProductID = 2, Quantity = 1, OrderStatus = "Shipped", CreatedOn = DateTime.Now }
            };
            _orderRepositoryMock.Setup(repo => repo.GetAllOrders()).ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderService.GetAllOrders();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOrders.Count, result.Count());
        }

        [Test]
        public async Task TestSearchOrders()
        {
            // Arrange
            string searchTerm = "ORD123";
            var expectedOrders = new List<Orders> {
                new Orders { ID = 1, UserId = 1, OrderNo = "ORD123", ProductID = 1, Quantity = 2, OrderStatus = "Placed", CreatedOn = DateTime.Now }
            };
            _orderRepositoryMock.Setup(repo => repo.SearchOrders(searchTerm)).ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderService.SearchOrders(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOrders.Count, result.Count());
        }

        [Test]
        public async Task TestGetUserOrders()
        {
            // Arrange
            int userId = 1;
            var expectedOrders = new List<Orders> {
                new Orders { ID = 1, UserId = userId, OrderNo = "ORD123", ProductID = 1, Quantity = 2, OrderStatus = "Placed", CreatedOn = DateTime.Now },
                new Orders { ID = 2, UserId = userId, OrderNo = "ORD124", ProductID = 2, Quantity = 1, OrderStatus = "Shipped", CreatedOn = DateTime.Now }
            };
            _orderRepositoryMock.Setup(repo => repo.GetUserOrders(userId)).ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderService.GetUserOrders(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOrders.Count, result.Count());
        }

        [Test]
        public async Task TestPlaceOrder()
        {
            // Arrange
            var newOrder = new Orders { UserId = 1, OrderNo = "ORD125", ProductID = 3, Quantity = 1, OrderStatus = "Placed", CreatedOn = DateTime.Now };
            _orderRepositoryMock.Setup(repo => repo.PlaceOrder(It.IsAny<Orders>())).ReturnsAsync(newOrder);

            // Act
            var result = await _orderService.PlaceOrder(newOrder);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newOrder.UserId, result.UserId);
            Assert.AreEqual(newOrder.OrderNo, result.OrderNo);
            Assert.AreEqual(newOrder.ProductID, result.ProductID);
            Assert.AreEqual(newOrder.Quantity, result.Quantity);
            Assert.AreEqual(newOrder.OrderStatus, result.OrderStatus);
            Assert.AreEqual(newOrder.CreatedOn, result.CreatedOn);
        }

        [Test]
        public async Task TestUpdateOrderStatus()
        {
            // Arrange
            int orderId = 1;
            string newOrderStatus = "Shipped";
            var updatedOrder = new Orders { ID = orderId, OrderStatus = newOrderStatus };

            _orderRepositoryMock.Setup(repo => repo.UpdateOrderStatus(orderId, newOrderStatus)).ReturnsAsync(updatedOrder);

            // Act
            var result = await _orderService.UpdateOrderStatus(orderId, newOrderStatus);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedOrder.ID, result.ID);
            Assert.AreEqual(updatedOrder.OrderStatus, result.OrderStatus);
        }

        [Test]
        public async Task TestDeleteOrder()
        {
            // Arrange
            int orderId = 2;

            _orderRepositoryMock.Setup(repo => repo.DeleteOrder(orderId)).ReturnsAsync(true);

            // Act
            var result = await _orderService.DeleteOrder(orderId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
