using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;


namespace ShopPC.Repository.ImplementationsRepository
{
    public class AttributesRepository: GenericReopository<Attributes>, IAttributesRepository
    {
        public AttributesRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Attributes>> GetAttributesByCategoryId(string categoryId)
        {
            return await _dbSet.Where(a => a.categoryId == categoryId).ToListAsync();
        }
    }
}
