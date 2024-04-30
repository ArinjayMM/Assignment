using Dapper;
using EcommerceWebAPI.Models;
using EcommerceWebAPI.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperDBContext _context;

        public OrderRepository(DapperDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            return await dbConnection.QueryAsync<Orders>("orderItems", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Orders>> GetUserOrders(int userId)
        {
            using (IDbConnection dbConnection = _context.CreateConnection())
            {
                var parameters = new { UserId = userId };
                var orders = await dbConnection.QueryAsync<Orders, Products, Orders>(
                    "GetUserOrders",
                    (order, product) =>
                    {
                        order.products = product;
                        return order;
                    },
                    parameters,
                    splitOn: "ID",
                    commandType: CommandType.StoredProcedure);

                return orders;
            }
        }

        public async Task<Orders> PlaceOrder(Orders order)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new
            {
                order.UserId,
                order.OrderNo,
                order.ProductID,
                order.Quantity,
                order.OrderStatus,
                order.CreatedOn
            };
            await dbConnection.ExecuteAsync("placeOrder", parameters, commandType: CommandType.StoredProcedure);
            return order;
        }

        public async Task<Orders> UpdateOrderStatus(int id, string orderStatus)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { ID = id, OrderStatus = orderStatus };
            return await dbConnection.QueryFirstOrDefaultAsync<Orders>("updateOrderStatus", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> DeleteOrder(int id)
        {
            using IDbConnection dbConnection = _context.CreateConnection();
            var parameters = new { Id = id };
            await dbConnection.ExecuteAsync("deleteOrder", parameters, commandType: CommandType.StoredProcedure);
            return true;
        }
    }
}
