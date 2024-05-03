using EcommerceWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Users> GetUserByEmail(string email);
        Task<Users> GetUserById(int id);
        Task<Users> AddUser(Users user);
        Task<Users> UpdateUser(string email, Users user);
    }
}
