using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class ProductImgMapper
    {
        public static ProductImg toProductImg(ProductImgRequest productImgRequest)
        {
            return new ProductImg
            {
                imgUrl = productImgRequest.imgUrl
            };
        }

        public static ProductImgResponse toProductImgResponse(ProductImg productImg)
        {
            return new ProductImgResponse
            {
                id = productImg.id,
                imgUrl = productImg.imgUrl,
                productId = productImg.productId
            };
        }
    }
}
