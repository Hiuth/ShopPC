using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IPcBuildItemService
    {
        Task<List<PcBuildItemResponse>> GetPcBuildItemsByPcBuildIdAsync(string pcBuildId);
        Task<PcBuildItemResponse?> GetPcBuildItemByIdAsync(string id);
        Task<PcBuildItemResponse> CreatePcBuildItemAsync(string pcBuildId, string productId,PcBuildItemRequest request);
        Task<PcBuildItemResponse> UpdatePcBuildItemAsync(string id, string? productId,PcBuildItemRequest request);
        Task<string> DeletePcBuildItemAsync(string id);
    }
}
