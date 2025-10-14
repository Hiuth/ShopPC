using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface ISubCategoryService
    {
        Task<SubCategoryResponse> createSubCategory(string categoryId, SubCategoryRequest request, IFormFile file);
        Task<SubCategoryResponse> updateSubCategory(string subCategoryId, string? categoryId,SubCategoryRequest request, IFormFile file);
        //Task<bool> deleteCategory(string id);
        Task<List<SubCategoryResponse>> getAllSubCategory();
        Task<SubCategoryResponse> getSubCategoryById(string id);
        Task<List<SubCategoryResponse>> getSubCategoryByCategoryId(string categoryId);
    }
}
