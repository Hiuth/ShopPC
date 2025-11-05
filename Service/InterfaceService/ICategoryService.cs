using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface ICategoryService
    {
        Task<CategoryResponse> createCategory(CategoryRequest request, IFormFile file );
        Task<CategoryResponse> updateCategory(string id, CategoryRequest request, IFormFile? file);
        //Task<bool> deleteCategory(string id);
        Task<List<CategoryResponse>> getAllCategory();
        Task<CategoryResponse> getCategoryById(string id);
        Task<CategoryRevenueResponse> getCategoryRevenueSummaryAsync();
    }
}
