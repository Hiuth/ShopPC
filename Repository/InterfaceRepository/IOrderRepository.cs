using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IOrderRepository: IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

        Task<String?> GetAccountIdByOrderIdAsync(string orderId);
    }
}
