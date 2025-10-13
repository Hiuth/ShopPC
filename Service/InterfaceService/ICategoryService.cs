using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface ICategoryService
    {
        Task<CategoryResponse> createCategory(CategoryRequest request);
        Task<CategoryResponse> updateCategory(string id, CategoryRequest request);
        //Task<bool> deleteCategory(string id);
        Task<List<CategoryResponse>> getAllCategory();
        Task<CategoryResponse> getCategoryById(string id);
    }
}
