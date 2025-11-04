using ShopPC.Models;
using System.Linq.Expressions;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface IProductUnitRepository: IGenericRepository<ProductUnit>
    {
        Task<List<ProductUnit>> GetProductUnitsByProductIdAsync(string productId);

        Task<ProductUnit?> GetProductUnitByImeiAsync(string imei);

        Task<ProductUnit?> GetProductUnitBySerialNumberAsync(string serialNumber);

        Task<ProductUnit?> GetProductUnitByIdAsync(string productUnitId);

        Task<bool> isImeiUniqueAsync(string imei);

        Task<bool> isSerialNumberUniqueAsync(string serialNumber);

        Task<int> CountProductUnitAvailableByProductIdAsync(string productId);

        Task<List<ProductUnit>> GetAvailableUnitsByProductId(string productId,int quantity);
    }
}
