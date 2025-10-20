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

        public async Task<(double avg, int total, int[] dist)> GetRatingSummaryByProductIdAsync(string productId)
        {
            var q = _dbSet.Where(c => c.productId == productId).Select(c => c.rating);

            var total = await q.CountAsync(); //lấy ra tổng số dữ liệu hợp lệ (số cmt đã rating tại bài đăng sản phẩm).
            var buckets = new int[6]; // dùng index 1..5

            if (total > 0)
            {
                var dist = await q.GroupBy(s => s)
                                  .Select(g => new { Star = g.Key, Cnt = g.Count() })
                                  .ToListAsync();
                foreach (var d in dist) buckets[d.Star] = d.Cnt; //lấy phân bố theo từng sao. (1 sao bao nhiêu cmt ,...)
            }

            double avg = total == 0 ? 0 // tính trung bình cộng 
                : (1.0 * (1 * buckets[1] + 2 * buckets[2] + 3 * buckets[3] + 4 * buckets[4] + 5 * buckets[5])) / total;

            return (avg, total, new[] { buckets[1], buckets[2], buckets[3], buckets[4], buckets[5] });
        }
    }
}
