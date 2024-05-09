using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

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
            Log.Information($"User with email {email} logged in successfully");
            return user;
        }

        public async Task<Users> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            Log.Information($"Getting user by ID: {id} => {user}");
            return user;
        }

        public async Task<Users> AddUser(Users user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var addUser = await _userRepository.AddUser(user);
            Log.Information($"User added => {addUser}");
            return addUser;
        }

        public async Task<Users> UpdateUser(string email, Users user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var updateUser = await _userRepository.UpdateUser(email, user);
            Log.Information($"Updated user with email: {email}");
            return updateUser;
        }
    }
}
