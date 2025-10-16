using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface ICartService
    {
        Task<CartResponse> AddToCart(string accountId,string productId,CartRequest request);
        Task<CartResponse> UpdateCart(string cartId, CartRequest request);
        Task<List<CartResponse>> GetCartByAccountId(string accountId);
        Task<string> ClearCart(string cartId);
        Task<string> ClearAllCart(string accountId);
    }
}
