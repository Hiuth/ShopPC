using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IPaymentRepository: IGenericRepository<PaymentLogs>
    {
        Task<PaymentLogs?> GetPaymentLogByOrderIdAsync(string orderId);


    }
}
