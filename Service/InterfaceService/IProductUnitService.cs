using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Repository.InterfaceRepository;
namespace ShopPC.Service.InterfaceService
{
    public interface IProductUnitService
    {
        Task<List<ProductUnitResponse>> GetProductUnitsByProductId(string productId);
        Task<ProductUnitResponse> GetProductUnitById(string productUnitId);
        Task<ProductUnitResponse> CreateProductUnit(string productId, ProductUnitRequest request);
        Task<ProductUnitResponse> UpdateProductUnit(string productUnitId, ProductUnitRequest request);
        Task<string> DeleteProductUnit(string productUnitId);
        
    }
}
