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
                quantity = request.quantity
            };
        }
        public static CartResponse toCartResponse(Cart cart)
        {
            return new CartResponse
            {
                id = cart.id,
                productId = cart.productId,
                quantity = cart.quantity,
                accountId = cart.accountId,
                price = cart.product.price,
                productName = cart.product.productName,
                thumbnail = cart.product.thumbnail??string.Empty,
                stockQuantity = cart.product.stockQuantity
            };
        }
    }
}
