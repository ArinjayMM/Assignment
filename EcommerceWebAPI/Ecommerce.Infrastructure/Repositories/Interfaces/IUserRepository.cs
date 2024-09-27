using EcommerceWebAPI.Models;

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
