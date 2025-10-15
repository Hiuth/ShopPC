using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
namespace ShopPC.Service.InterfaceService
{
    public interface IProductImgService
    {
        Task<ProductImgResponse> CreateProductImg(string productId, IFormFile file);

        Task<string> DeleteProductImg(string productImgId);

        Task<List<ProductImgResponse>> GetProductImgByProductId(string productId);
    }
}
