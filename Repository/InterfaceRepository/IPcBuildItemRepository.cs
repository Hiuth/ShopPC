using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IPcBuildItemRepository: IGenericRepository<PcBuildItem>
    {
        Task<List<PcBuildItem>> GetPcBuildItemsByPcBuildIdAsync(string pcBuildId);

        Task<PcBuildItem?> GetPcBuildItemByIdAsync(string pcBuildId);

        Task<bool> isProductIdInPcBuildItemAsync(string productId);

        Task<List<PcBuildItem>> GetPcBuildItemByProductIdAsync(string productId);

        //Task<string> DeletePcBuildItemsByPcBuildIdAsync(string pcBuildId);
    }
}
