using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;

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

        public async Task<CategoryRevenueResponse> GetCategoryRevenueSummaryAsync()
        {
            var categories = await _dbSet
                .Select(c => new CategoryRevenueItemResponse
                {
                    CategoryId = c.id,
                    CategoryName = c.categoryName,
                    TotalRevenue = c.products
                        .SelectMany(p => p.orderDetails)
                        .Where(od => od.order.status == "DELIVERED")
                        .Sum(od => (decimal?)od.quantity * od.unitPrice) ?? 0,

                    OrderCount = c.products
                        .SelectMany(p => p.orderDetails)
                        .Where(od => od.order.status == "DELIVERED")
                        .Select(od => od.orderId)
                        .Distinct()
                        .Count()
                })
                .ToListAsync();

            return new CategoryRevenueResponse
            {
                Categories = categories,
                TotalRevenue = categories.Sum(x => x.TotalRevenue)
            };
        }
    }
}
