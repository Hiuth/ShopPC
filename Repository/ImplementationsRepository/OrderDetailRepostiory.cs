using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class OrderDetailRepostiory : GenericReopository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepostiory(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string orderId)
        {
            return await _dbSet
                .Include(od => od.product)
                .Where(od => od.orderId == orderId).ToListAsync();
        }
    }
}
