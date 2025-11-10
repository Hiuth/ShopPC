using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.Models;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class PaymentRepository: GenericReopository<PaymentLogs>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }
        
        public async Task<PaymentLogs?> GetPaymentLogByOrderIdAsync(string orderId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.orderId == orderId);
        }
    }
}
