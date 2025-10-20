using ShopPC.DTO.Request;   
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(string accountId, OrderRequest request);
        Task<OrderResponse> UpdateOrder(string orderId, OrderRequest request);
        Task<List<OrderResponse>> GetOrdersByAccountId(string accountId);
        Task<List<OrderResponse>> GetAllOrders();
        //Task<OrderResponse> GetOrderById(string orderId);
        Task<string> DeleteOrder(string orderId);
    }
}
