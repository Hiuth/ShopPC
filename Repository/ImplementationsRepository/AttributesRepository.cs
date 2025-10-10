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

        public async Task<List<Attributes>> GetAttributesBySubCategoryId(string subCategoryId)
        {
            var attributes = await _context.Attributes
                .Where(a => a.subCategoryId == subCategoryId)
                .ToListAsync();
            return attributes;
        }
    }
}
