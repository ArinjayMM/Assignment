using EcommerceWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace EcommerceWebAPI.Repositories
{
    public class DapperDBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connection;

        public DapperDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connection);
    }
}
