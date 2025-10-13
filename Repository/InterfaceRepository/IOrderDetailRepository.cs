using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IOrderDetailRepository: IGenericRepository<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string orderId);
    }
}
