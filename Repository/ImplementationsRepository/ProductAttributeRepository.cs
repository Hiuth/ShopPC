using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class ProductAttributeRepository: GenericReopository<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<ProductAttribute>> GetProductAttributesByProductIdAsync(string productId)
        {
            return await _dbSet
                .Include(p=> p.attribute)
                .Where(attr => attr.productId == productId).ToListAsync();
        }

        public async Task<ProductAttribute?> GetProductAttributeByIdAsync(string productAttributeId)
        {
            return await _dbSet
                .Include(p => p.attribute)
                .FirstOrDefaultAsync(attr => attr.id == productAttributeId);
        }
    }
}
