using ShopPC.Models;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface IProductAttributeRepository: IGenericRepository<ProductAttribute>
    {
        Task<List<ProductAttribute>> GetProductAttributesByProductIdAsync(string productId);
    }
}
