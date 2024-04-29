using EcommerceWebAPI.Data;
using EcommerceWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _context;

        public UserRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Users> GetUserByEmail(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users> AddUser(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> UpdateUser(string email, Users user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Status = user.Status;
                await _context.SaveChangesAsync();
            }
            return existingUser;
        }
    }
}
