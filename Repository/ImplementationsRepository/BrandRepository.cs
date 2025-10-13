using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class BrandRepository : GenericReopository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsBrandNameUniqueAsync(string brandName)
        {
            return !await _dbSet.AnyAsync(b => b.brandName == brandName);
        }

        public async Task<IEnumerable<Brand>> GetBrandByCategoryIdAsync(string categoryId)
        {
            return await _dbSet.Where(p => p.categoryId == categoryId).ToListAsync();
        }
    }
}
