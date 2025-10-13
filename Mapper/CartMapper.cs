using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class CartMapper
    {
        public static Cart toCart(CartRequest request)
        {
            return new Cart
            {
                productId = request.productId,
                quantity = request.quantity,
                accountId = request.accountId
            };
        }
        public static CartResponse toCartResponse(Cart cart)
        {
            return new CartResponse
            {
                id = cart.id,
                productId = cart.productId,
                quantity = cart.quantity,
                accountId = cart.accountId
            };
        }
    }
}
