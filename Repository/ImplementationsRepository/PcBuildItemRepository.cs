using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class PcBuildItemRepository: GenericReopository<PcBuildItem>, IPcBuildItemRepository
    {
        public PcBuildItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<PcBuildItem>> GetPcBuildItemsByPcBuildIdAsync(string pcBuildId)
        {
            return await _dbSet
                 .Include(item => item.product)
                 .Include(item => item.pcBuild)
                .Where(item => item.pcBuildId == pcBuildId)
                .ToListAsync();
        }

        public async Task<PcBuildItem?> GetPcBuildItemByIdAsync(string id)
        {
            return await _context.PcBuildItems
                .Include(item => item.product)
                .Include(item => item.pcBuild)
                .FirstOrDefaultAsync(item => item.id == id);
        }
    }
}
