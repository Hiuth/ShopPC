using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(string brandId,string subCategoryId ,ProductRequest request, IFormFile file);
        Task<ProductResponse> UpdateProduct(string productId,string? brandId,string? subCategoryId, ProductRequest request, IFormFile? file);
        //Task<bool> DeleteProduct(string id);
        Task<IEnumerable<ProductResponse>> GetAllProduct();
        Task<ProductResponse> GetProductById(string id);
        Task<IEnumerable<ProductResponse>> GetProductsBySubCategoryId(string subCategoryId);
        Task<IEnumerable<ProductResponse>> GetProductsByBrandId(string brandId);
        Task<IEnumerable<ProductResponse>> SearchProducts(string searchTerm);
        Task<IEnumerable<ProductResponse>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);

    }
}
