using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using EcommerceWebAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users> GetUserByEmail(string email, string password)
        {
            return await _userRepository.GetUserByEmail(email, password);
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<Users> AddUser(Users user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<Users> UpdateUser(string email, Users user)
        {
            return await _userRepository.UpdateUser(email, user);
        }
    }
}