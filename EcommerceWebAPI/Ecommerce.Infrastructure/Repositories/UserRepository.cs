using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;

namespace EcommerceWebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperDBContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DapperDBContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Users> GetUserById(int id)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Id = id };
            return await dbConnection.QueryFirstOrDefaultAsync<Users>("userById", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<Users> GetUserByEmail(string email)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Email = email };
            return await dbConnection.QueryFirstOrDefaultAsync<Users>("userByEmail", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<Users> AddUser(Users user)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password
            };
            await dbConnection.ExecuteAsync("insertUser", parameters, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task<Users> UpdateUser(string email, Users user)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password
            };
            await dbConnection.ExecuteAsync("updateUser", parameters, commandType: CommandType.StoredProcedure);
            return user;
        }
    }
}
