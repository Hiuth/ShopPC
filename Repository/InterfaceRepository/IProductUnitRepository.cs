using ShopPC.Models;
using System.Linq.Expressions;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface IProductUnitRepository: IGenericRepository<ProductUnit>
    {
        Task<List<ProductUnit>> GetProductUnitsByProductIdAsync(string productId);
    }
}
