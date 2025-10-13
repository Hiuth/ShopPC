using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class OrderDetailMapper
    {
        public static OrderDetail toOrderDetail(OrderDetailRequest request)
        {
            return new OrderDetail
            {
                orderId = request.orderId,
                productId = request.productId,
                quantity = request.quantity,
                unitPrice = request.unitPrice
            };
        }
        public static OrderDetailResponse toOrderDetailResponse(OrderDetail orderDetail)
        {
            return new OrderDetailResponse
            {
                id = orderDetail.id,
                orderId = orderDetail.orderId,
                productId = orderDetail.productId,
                quantity = orderDetail.quantity,
                unitPrice = orderDetail.unitPrice
            };
        }
    }
}
