using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.Models;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
namespace ShopPC.Repository.ImplementationsRepository
{
    public class ProductUnitRepository: GenericReopository<ProductUnit>,IProductUnitRepository
    {
        public ProductUnitRepository(AppDbContext context): base(context)
        {
        }

        public async Task<List<ProductUnit>> GetProductUnitsByProductIdAsync(string productId)
        {
            return await _dbSet
                .Include(pu => pu.product)
                .Where(pu => pu.productId == productId)
                .ToListAsync();
        }

        public async Task<ProductUnit?> GetProductUnitByImeiAsync(string imei)
        {
            return await _dbSet
                .Include(pu => pu.product)
                .FirstOrDefaultAsync(pu => pu.imei == imei);
        }

        public async Task<ProductUnit?> GetProductUnitBySerialNumberAsync(string serialNumber)
        {
            return await _dbSet
                .Include(pu => pu.product)
                .FirstOrDefaultAsync(pu => pu.serialNumber == serialNumber);
        }

        public async Task<ProductUnit?> GetProductUnitByIdAsync(string productUnitId)
        {
            return await _dbSet
                .Include(pu => pu.product)
                .FirstOrDefaultAsync(pu => pu.id == productUnitId);
        }

        public async Task<bool> isImeiUniqueAsync(string imei)
        {
            return !await _dbSet.AnyAsync(pu => pu.imei == imei);
        }

        public async Task<bool> isSerialNumberUniqueAsync(string serialNumber)
        {
            return !await _dbSet.AnyAsync(pu => pu.serialNumber == serialNumber);
        }

        public async Task<int> CountProductUnitAvailableByProductIdAsync(string productId)
        {
            return await _dbSet
                .Where(pu => pu.productId == productId && pu.status == "AVAILABLE")
                .CountAsync();
        }

        public async Task<List<ProductUnit>> GetAvailableUnitsByProductId(string productId, int quantity)
        {
            return await _dbSet
                .Where(pu => pu.productId == productId && pu.status == "AVAILABLE")
                .Take(quantity)
                .ToListAsync();
        }
    }
}
