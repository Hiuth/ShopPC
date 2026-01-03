using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class OrderRepository: GenericReopository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _dbSet.Where(o => o.accountId == userId).ToListAsync();
        }

        public async Task<String?> GetAccountIdByOrderIdAsync(string orderId)
        {
            var order = await _dbSet.FirstOrDefaultAsync(o => o.id == orderId);
            return order != null ? order.accountId : null;
        }

    }
}
