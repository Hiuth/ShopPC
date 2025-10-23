using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;


namespace ShopPC.Repository.ImplementationsRepository
{
    public class PcBuildRepository : GenericReopository<PcBuild>,IPcBuildRepository
    {
        public PcBuildRepository(AppDbContext context): base(context)
        {
        }

        //public async Task<PcBuild?> GetPcBuildByIdWithItemsAsync(string id)
        //{
        //    return await _dbSet
        //        .Include(pb => pb.pcBuildItem)
        //        .ThenInclude(pbi => pbi.product)
        //        .FirstOrDefaultAsync(pb => pb.id == id);
        //}

        public async Task<PcBuild?> GetPcBuildByIdAsync(string id)
        {
            return await _dbSet
                .Include(pb => pb.subCategory)
                .FirstOrDefaultAsync(pb => pb.id == id);
        }

        public async Task<List<PcBuild>> GetPcBuildsBySubCategoryIdAsync(string subCategoryId)
        {
            return await _dbSet
                .Include(pb => pb.subCategory)
                .Where(pb => pb.subCategoryId == subCategoryId)
                .ToListAsync();
        }

        public async Task<List<PcBuild>> GetAllPcBuildAsync()
        {
            return await _dbSet
                .Include(pb => pb.subCategory)
                .ToListAsync();
        }
    }
}
