using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Users> GetUserByEmail(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            Console.WriteLine(email, password);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                _logger.LogError($"Login attempt failed for user with email: {email}");
                return null;
            }
            _logger.LogInformation($"User with email {email} logged in successfully");
            return user;
        }

        public async Task<Users> GetUserById(int id)
        {
            _logger.LogInformation($"Getting user by ID: {id}");
            return await _userRepository.GetUserById(id);
        }

        public async Task<Users> AddUser(Users user)
        {
            _logger.LogInformation("Adding user...");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            return await _userRepository.AddUser(user);
        }

        public async Task<Users> UpdateUser(string email, Users user)
        {
            _logger.LogInformation($"Updating user with email: {email}");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            return await _userRepository.UpdateUser(email, user);
        }
    }
}
