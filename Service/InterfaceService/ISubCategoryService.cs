using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface ISubCategoryService
    {
        Task<SubCategoryResponse> createSubCategory(SubCategoryRequest request, IFormFile file);
        Task<SubCategoryResponse> updateSubCategory(string id, SubCategoryRequest request, IFormFile file);
        //Task<bool> deleteCategory(string id);
        Task<List<SubCategoryResponse>> getAllSubCategory();
        Task<SubCategoryResponse> getSubCategoryById(string id);
        Task<List<SubCategoryResponse>> getSubCategoryByCategoryId(string categoryId);
    }
}
