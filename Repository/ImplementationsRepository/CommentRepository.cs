using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class CommentRepository: GenericReopository<Comment>,ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Comment>> GetCommentByProductIdAsync(string productId)
        {
            return await _dbSet
                .Include(c => c.account)
                .Where(c => c.productId == productId).ToListAsync();
        }
        public async Task<List<Comment>> GetCommentByAccountIdAsync(string accountId)
        {
            return await _dbSet
                .Include(c => c.account)
                .Where(c => c.accountId == accountId).ToListAsync();
        }
    }
}
