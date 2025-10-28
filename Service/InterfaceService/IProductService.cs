using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(string brandId,string? subCategoryId ,ProductRequest request, IFormFile file);
        Task<ProductResponse> UpdateProduct(string productId,string? brandId,string? subCategoryId, ProductRequest request, IFormFile? file);
        Task<ProductResponse> GetProductById(string id);
        //Task<bool> DeleteProduct(string id);
        Task<PaginatedResponse<ProductResponse>> GetAllProduct(int pageNumber, int pageSize);
        Task<PaginatedResponse<ProductResponse>> GetProductsBySubCategoryId(string subCategoryId, int pageNumber, int pageSize);
        Task<PaginatedResponse<ProductResponse>> GetProductsByBrandId(string brandId, int pageNumber, int pageSize);
        Task<PaginatedResponse<ProductResponse>> SearchProducts(string searchTerm, int pageNumber, int pageSize);
        Task<PaginatedResponse<ProductResponse>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice, int pageNumber, int pageSize);

    }
}
