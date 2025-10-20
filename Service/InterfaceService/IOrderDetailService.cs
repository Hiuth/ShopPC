using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IOrderDetailService
    {
        Task<OrderDetailResponse> CreateOrderDetail(string orderId, string productId,OrderDetailRequest request);
        Task<List<OrderDetailResponse>> GetOrderDetailByOrderId(string orderId);
        Task<string> DeleteOrderDetailbyOrderId(string orderId);
    }
}
