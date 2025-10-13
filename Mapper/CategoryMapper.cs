using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;


namespace ShopPC.Mapper
{
    public class CategoryMapper
    {
        public static Category toCategory(CategoryRequest categoryRequest)
        {
            return new Category
            {
                categoryName = categoryRequest.categoryName,
                iconImg = categoryRequest.iconImg
            };
        }

        public static CategoryResponse toCategoryResponse(Category category)
        {
            return new CategoryResponse 
            { 
                id = category.id,
                categoryName = category.categoryName,
                iconImg = category.iconImg
            };
        }
    }
}
