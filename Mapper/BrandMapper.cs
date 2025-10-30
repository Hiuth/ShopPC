using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class BrandMapper
    {
        public static Brand toBrand(BrandRequest brandRequest)
        {
            return new Brand
            { 
                brandName = brandRequest.brandName
            };
        }

        public static BrandResponse toBrandResponse(Brand brand)
        {
            return new BrandResponse
            {
                id = brand.id,
                brandName = brand.brandName,
            };
        }
    }
}
