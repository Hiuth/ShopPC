using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class PcBuildMapper
    {
        public static PcBuild toPcBuild(PcBuildRequest pcBuildRequest)
        {
            return new PcBuild
            {
                productName = pcBuildRequest.productName,
                price = pcBuildRequest.price,
                description = pcBuildRequest.description,
                status = pcBuildRequest.status,
            };
        }

        public static PcBuildResponse toPcBuildResponse(PcBuild response)
        {
            return new PcBuildResponse
            {
                id = response.id,
                productName = response.productName,
                price = response.price ?? 0,
                description = response.description,
                thumbnail = response.thumbnail!,
                status = response.status,
                subCategoryId = response.subCategoryId,
                subCategoryName = response.subCategory.subCategoryName
            };
        }
    }
}
