using ShopPC.Models;
using System.Linq.Expressions;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IPcBuildRepository: IGenericRepository<PcBuild>
    {
        Task<PcBuild?> GetPcBuildByIdAsync(string id);
     //   Task<PcBuild?> GetPcBuildByIdWithItemsAsyncAsync(string id);
        Task<List<PcBuild>> GetPcBuildsBySubCategoryIdAsync(string subCategoryId);

        Task<List<PcBuild>> GetAllPcBuildAsync();
    }
}
