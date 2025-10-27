using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface ICartService
    {
        Task<CartResponse> AddToCart(string productId,CartRequest request);
        Task<CartResponse> UpdateCart(string cartId, CartRequest request);
        Task<List<CartResponse>> GetCartByAccountId();
        Task<string> ClearCart(string cartId);
        Task<string> ClearAllCart();
    }
}
