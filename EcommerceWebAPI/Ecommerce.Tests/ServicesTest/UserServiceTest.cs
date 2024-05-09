using NUnit.Framework;
using Moq;
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Services;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Tests.ServicesTest
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ILogger<UserService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetUserByEmail_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var password = "testpassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var expectedUser = new Users
            {
                ID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                Password = hashedPassword
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByEmail(email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.ID, result.ID);
            Assert.AreEqual(expectedUser.FirstName, result.FirstName);
            Assert.AreEqual(expectedUser.LastName, result.LastName);
            Assert.AreEqual(expectedUser.Email, result.Email);
            Assert.AreEqual(expectedUser.Password, result.Password);
        }

        [Test]
        public async Task GetUserByEmail_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongpassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
            var expectedUser = new Users
            {
                ID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                Password = hashedPassword
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByEmail(email, password);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserByEmail_WithNonExistentEmail_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";

            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync((Users)null);

            // Act
            var result = await _userService.GetUserByEmail(email, "anypassword");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new Users
            {
                ID = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("testpassword")
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.ID, result.ID);
            Assert.AreEqual(expectedUser.FirstName, result.FirstName);
            Assert.AreEqual(expectedUser.LastName, result.LastName);
            Assert.AreEqual(expectedUser.Email, result.Email);
            Assert.AreEqual(expectedUser.Password, result.Password);
        }

        [Test]
        public async Task GetUserById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var userId = 10;

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((Users)null);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddUser_ReturnsAddedUser()
        {
            // Arrange
            var newUser = new Users
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Password = "testpassword"
            };

            _userRepositoryMock.Setup(repo => repo.AddUser(newUser)).ReturnsAsync(newUser);

            // Act
            var result = await _userService.AddUser(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newUser.ID, result.ID);
            Assert.AreEqual(newUser.FirstName, result.FirstName);
            Assert.AreEqual(newUser.LastName, result.LastName);
            Assert.AreEqual(newUser.Email, result.Email);
            Assert.AreEqual(newUser.Password, result.Password);
        }

        [Test]
        public async Task UpdateUser_WithValidEmail_ReturnsUpdatedUser()
        {
            // Arrange
            var email = "jane@example.com";
            var updatedUser = new Users
            {
                ID = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = email,
                Password = "updatedpassword"
            };

            _userRepositoryMock.Setup(repo => repo.UpdateUser(email, updatedUser)).ReturnsAsync(updatedUser);

            // Act
            var result = await _userService.UpdateUser(email, updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.ID, result.ID);
            Assert.AreEqual(updatedUser.FirstName, result.FirstName);
            Assert.AreEqual(updatedUser.LastName, result.LastName);
            Assert.AreEqual(updatedUser.Email, result.Email);
            Assert.AreEqual(updatedUser.Password, result.Password);
        }

        [Test]
        public async Task UpdateUser_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var updatedUser = new Users
            {
                ID = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = email,
                Password = "updatedpassword"
            };

            _userRepositoryMock.Setup(repo => repo.UpdateUser(email, updatedUser)).ReturnsAsync((Users)null);

            // Act
            var result = await _userService.UpdateUser(email, updatedUser);

            // Assert
            Assert.IsNull(result);
        }
    }
}
