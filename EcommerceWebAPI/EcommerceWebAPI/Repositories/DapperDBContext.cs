using EcommerceWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebAPI.Repositories
{
    public class DapperDBContext : DbContext
    {
        public DapperDBContext(DbContextOptions<DapperDBContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Orders> Orders { get; set; }
    }
}
