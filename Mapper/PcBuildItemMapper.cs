using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class PcBuildItemMapper
    {
        public static PcBuildItem toPcBuildItem(PcBuildItemRequest pcBuildItemRequest)
        {
            return new PcBuildItem
            {
                quantity = pcBuildItemRequest.quantity,
            };
        }
        public static PcBuildItemResponse toPcBuildItemResponse(PcBuildItem response)
        {
            return new PcBuildItemResponse
            {
                id = response.id,
                pcBuildId = response.pcBuildId,
                productId = response.productId,
                quantity = response.quantity,
                productName = response.product.productName,
                price = response.product.price ?? 0,
                thumbnail = response.product.thumbnail!,
                stockQuantity = response.product.stockQuantity ??0,
            };
        }
    }
}
