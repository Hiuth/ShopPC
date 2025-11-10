using ShopPC.Models;

namespace ShopPC.Service.InterfaceService
{
    public interface IPaymentService
    {
        string CreatePaymentUrl(string orderId, decimal amount, string description);

        bool ValidateResponse(IDictionary<string, string> queryParams);

        string BuildFrontendRedirectUrl(IDictionary<string, string> vnPayQueryParams, string clientBaseUrl);
        
        Task SavePaymentLogAsync(string orderId, decimal amount, string status, string transactionId);
    }
}
