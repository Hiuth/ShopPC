using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IPcBuildService
    {
        Task<PcBuildResponse> CreatePcBuild(string subCateoryId, PcBuildRequest request, IFormFile file);
        Task<PcBuildResponse> UpdatePcBuild(string pcBuildId,string subCategoryId ,PcBuildRequest request, IFormFile file);
        Task<PcBuildResponse> GetPcBuildById(string id);
        Task<string> DeletePcBuild(string id);
        Task<PaginatedResponse<PcBuildResponse>> GetAllPcBuilds(int pageNumber, int pageSize);
        Task<PaginatedResponse<PcBuildResponse>> GetPcBuildsBySubCategoryId(string subCategoryId, int pageNumber, int pageSize);

    }
}
