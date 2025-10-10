using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IProductImgRepository: IGenericRepository<ProductImg>
    {
        Task<IEnumerable<ProductImg>> GetImagesByProductIdAsync(string productId);
    }
}
