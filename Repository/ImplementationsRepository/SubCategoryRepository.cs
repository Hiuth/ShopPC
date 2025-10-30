using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class SubCategoryRepository: GenericReopository<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsSubCategoryNameUniqueAsync(string subCategoryName)
        {
            return !await _dbSet.AnyAsync(sc => sc.subCategoryName == subCategoryName);
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(string categoryId)
        {
            return await _dbSet
                .Include(sc => sc.category)
                .Where(sc => sc.categoryId == categoryId).ToListAsync();
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(string subCategoryId)
        {
            return await _dbSet
                .Include(sc => sc.category)
                .FirstOrDefaultAsync(sc => sc.id == subCategoryId);
        }

        public async Task<List<SubCategory>> GetAllSubCategoryAsync()
        {
            return await _dbSet
                .Include(sc => sc.category)
                .ToListAsync();
        }

    }
}
