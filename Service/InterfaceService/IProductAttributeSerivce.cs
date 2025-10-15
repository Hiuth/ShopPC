using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IProductAttributeSerivce
    {
        Task<ProductAttributeResponse> CreateProductAttribute(string attributeId, string productId, ProductAttributeRequest request);

        Task<ProductAttributeResponse> UpdateProductAttribute(string productAttributeId, string? attributeId, ProductAttributeRequest request);

        Task<List<ProductAttributeResponse>> GetProductAttributeByProductId(string productId);

        Task<string> DeleteProductAttribute(string productAttributeId);
    }
}
