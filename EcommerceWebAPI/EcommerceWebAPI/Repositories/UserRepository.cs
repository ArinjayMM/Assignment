using Dapper;
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperDBContext _context;

        public UserRepository(DapperDBContext context)
        {
            _context = context;
        }

        public async Task<Users> GetUserById(int id)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Id = id };
            return await dbConnection.QueryFirstOrDefaultAsync<Users>("userById", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<Users> GetUserByEmail(string email, string password)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Email = email, Password = password };
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
