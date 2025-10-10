using ShopPC.Models;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface IProductAttributeRepository: IGenericRepository<ProductAttribute>
    {
        Task<IEnumerable<ProductAttribute>> GetProductAttributesByProductIdAsync(string productId);
    }
}
