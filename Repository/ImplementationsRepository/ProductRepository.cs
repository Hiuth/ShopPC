using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class ProductRepository : GenericReopository<Products>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Products>> GetProductsBySubCategoryIdAsync(string SubCategoryId)
        {
            return await _dbSet
                .Include(p => p.subCategory)
                .Include(p => p.brand)
                .Where(p => p.subCategoryId == SubCategoryId).ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetProductsByBrandIdAsync(string brandId)
        {
            return await _dbSet
                .Include(p => p.subCategory)
                .Include(p => p.brand)
                .Where(p => p.brandId == brandId).ToListAsync();
        }

        public async Task<IEnumerable<Products>> SearchProductsAsync(string searchTerm)// phải nâng cấp sớm
        {
            return await _dbSet
                 .Include(p => p.subCategory)
                .Include(p => p.brand)
                .Where(p => p.productName.Contains(searchTerm)).ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                 .Include(p => p.subCategory)
                .Include(p => p.brand)
                .Where(p => p.price >= minPrice && p.price <= maxPrice).ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetAllProductAsync()
        {
            return await _dbSet
                .Include(p => p.subCategory)
                .Include(p => p.brand)
                .ToListAsync();
        }

        public async Task<Products?> GetProductByIdAsync(string id)
        {
            return await _dbSet
                .Include(p => p.subCategory)
                .Include(p => p.brand)
                .FirstOrDefaultAsync(p => p.id == id);
        }
    }
}
