using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.Models;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
namespace ShopPC.Service.ImplementationsService
{
    public class ProductUnitRepository: GenericReopository<ProductUnit>,IProductUnitRepository
    {
        private ProductUnitRepository(AppDbContext context): base(context)
        {
        }

        public async Task<List<ProductUnit>> GetProductUnitsByProductIdAsync(string productId)
        {
            return await _dbSet
                .Where(pu => pu.productId == productId)
                .ToListAsync();
        }
    }
}
