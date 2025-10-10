using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class CategoryRepository : GenericReopository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string categoryName)
        {
            return !await _dbSet.AnyAsync(c => c.categoryName == categoryName);
        }
    }
}
