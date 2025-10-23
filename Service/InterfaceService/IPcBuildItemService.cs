using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IPcBuildItemService
    {
        Task<List<PcBuildItemResponse>> GetPcBuildItemsByPcBuildId(string pcBuildId);
        Task<PcBuildItemResponse?> GetPcBuildItemById(string id);
        Task<PcBuildItemResponse> CreatePcBuildItem(string pcBuildId, string productId,PcBuildItemRequest request);
        Task<PcBuildItemResponse> UpdatePcBuildItem(string id, string? productId,PcBuildItemRequest request);
        Task<string> DeletePcBuildItem(string id);
    }
}
