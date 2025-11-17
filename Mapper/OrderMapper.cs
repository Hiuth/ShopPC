using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class OrderMapper
    {
        public static Order toOrder(OrderRequest request)
        {
            return new Order
            {
                status = request.status,
                totalAmount = request.totalAmount,
                CustomerName = request.customerName,
                phoneNumber = request.phoneNumber,
                address = request.address
            };
        }
        public static OrderResponse toOrderResponse(Order order)
        {
            return new OrderResponse
            {
                id = order.id,
                orderDate = order.orderDate,
                status = order.status,
                accountId = order.accountId,
                totalAmount = order.totalAmount,
                CustomerName = order.CustomerName,
                phoneNumber = order.phoneNumber,
                address = order.address,
                isPaid = order.isPaid
            };
        }
    }
}
