using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IAttributesService
    {
        Task<AttributesResponse> CreateAttribute(string categoryId,AttributesRequest request);
        Task<AttributesResponse> UpdateAttribute(string attributeId,string? categoryId ,AttributesRequest request);
        //Task<bool> DeleteAttribute(int id);
        Task<AttributesResponse> GetAttributeById(string id);
        Task<List<AttributesResponse>> GetAllAttributes();
        Task<List<AttributesResponse>> GetAttributesByCategoryId(string categoryId);
    }
}
