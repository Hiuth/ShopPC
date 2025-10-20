using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class ProductUnitMapper
    {
        public static ProductUnit toProductUnit(ProductUnitRequest request)
        {
            return new ProductUnit
            {
                imei = request.imei,
                status = request.status,
                serialNumber = request.serialNumber
            };
        }

        public static ProductUnitResponse toProductUnitResponse(ProductUnit productUnit)
        {
            return new ProductUnitResponse
            {
                id = productUnit.id,
                productId = productUnit.productId,
                productName = productUnit.product.productName ?? string.Empty,
                imei = productUnit.imei,
                status = productUnit.status,
                serialNumber = productUnit.serialNumber
            };
        }
    }
}
