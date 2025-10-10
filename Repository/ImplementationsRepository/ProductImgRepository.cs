using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;


namespace ShopPC.Repository.ImplementationsRepository
{
    public class ProductImgRepository: GenericReopository<ProductImg>, IProductImgRepository
    {
        public ProductImgRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ProductImg>> GetImagesByProductIdAsync(string productId)
        {
            return await _dbSet.Where(img => img.productId == productId).ToListAsync();
        }
    }
}
