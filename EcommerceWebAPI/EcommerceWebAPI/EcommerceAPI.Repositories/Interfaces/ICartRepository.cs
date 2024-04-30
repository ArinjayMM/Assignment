using EcommerceWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceWebAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllCartItems();
        Task<IEnumerable<Cart>> GetCartItems(int userId);
        Task<Cart> AddtoCart(Cart cart);
        Task<Cart> UpdateCartItem(int id, Cart cart);
        Task<bool> DeleteCartItem(int id);
    }
}
