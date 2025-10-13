using ShopPC.Models;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Mapper
{
    public class SubCategoryMapper
    {
        public static SubCategory toSubCategory(SubCategoryRequest subCategoryRequest)
        {
            return new SubCategory
            {
                subCategoryName = subCategoryRequest.subCategoryName,
                subCategoryImg = subCategoryRequest.subCategoryImg,
                description = subCategoryRequest.description,
                categoryId = subCategoryRequest.categoryId
            };
        }

        public static SubCategoryResponse toSubCategoryResponse(SubCategory subCategory)
        {
            return new SubCategoryResponse
            {
                id = subCategory.id,
                subCategoryName = subCategory.subCategoryName,
                subCategoryImg = subCategory.subCategoryImg,
                description = subCategory.description,
                categoryId = subCategory.categoryId,
            };
        }
    }
}
