using EcommerceWebAPI.Models;

namespace EcommerceWebAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Orders>> GetAllOrders();
        Task<IEnumerable<Orders>> SearchOrders(string searchTerm);
        Task<IEnumerable<Orders>> GetUserOrders(int userId);
        Task<Orders> PlaceOrder(Orders order);
        Task<Orders> UpdateOrderStatus(int id, string orderStatus);
        Task<bool> DeleteOrder(int id);
    }
}
