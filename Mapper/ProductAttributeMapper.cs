using ShopPC.Models;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
namespace ShopPC.Mapper
{
    public class ProductAttributeMapper
    {
        public static ProductAttribute toProductAttribute(ProductAttributeRequest productAttributeRequest)
        {
            return new ProductAttribute
            {
                attributeId = productAttributeRequest.attributeId,
                value = productAttributeRequest.value,
                productId = productAttributeRequest.productId
            };
        }

        public static ProductAttributeResponse toProductAttributeResponse(ProductAttribute productAttribute)
        {
            return new ProductAttributeResponse
            {
                id = productAttribute.id,
                attributeId = productAttribute.attributeId,
                value = productAttribute.value,
                productId = productAttribute.productId
            };
        }
    }
}
