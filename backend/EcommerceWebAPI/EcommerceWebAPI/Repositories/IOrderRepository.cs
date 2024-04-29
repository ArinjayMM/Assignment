using EcommerceWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllOrders();
        Task<IEnumerable<Orders>> GetUserOrders(int userId);
        Task<Orders> PlaceOrder(Orders order);
        Task<Orders> UpdateOrderStatus(int id, string orderStatus);
        Task<bool> DeleteOrder(int id);
    }
}
