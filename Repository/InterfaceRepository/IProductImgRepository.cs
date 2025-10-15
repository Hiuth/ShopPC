using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IProductImgRepository: IGenericRepository<ProductImg>
    {
        Task<List<ProductImg>> GetImagesByProductIdAsync(string productId);
    }
}
