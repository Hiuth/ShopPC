using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class AttributesMapper
    {
        public static Attributes toAttributes(AttributesRequest attributesRequest)
        {
            return new Attributes
            {
                attributeName = attributesRequest.attributeName
            };
        }

        public static AttributesResponse toAttributesResponse(Attributes attributes)
        {
            return new AttributesResponse
            {
                id = attributes.id,
                attributeName = attributes.attributeName,
                categoryId = attributes.categoryId
            };
        }
    }
}
