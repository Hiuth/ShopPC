using ShopPC.Models;
using System.Linq.Expressions;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface ICommentRepository: IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentByProductIdAsync(string productId);
        Task<List<Comment>> GetCommentByAccountIdAsync(string accountId);
        Task<Comment?> GetCommentByCommentIdAsync(string commentId); 
        Task<(double avg, int total, int[] dist)> GetRatingSummaryByProductIdAsync(string productId);
    }
}
