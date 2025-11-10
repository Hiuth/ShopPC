using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShopPC.Configuration;
using ShopPC.Models;
using ShopPC.Repository.InterfaceRepository;
using ShopPC.Service.InterfaceService;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ShopPC.Service.ImplementationsService
{
    public class PaymentService: IPaymentService
    {
        private readonly VnPayConfig _vnpayConfig;
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IOptions<VnPayConfig> vnpayConfig, IPaymentRepository paymentRepository)
        {
            _vnpayConfig = vnpayConfig.Value;
            _paymentRepository = paymentRepository;
        }

        private string BuildDataToSign(IDictionary<string, string> data)
        {
            return string.Join("&", data.OrderBy(x => x.Key)
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .Select(x => $"{x.Key}={x.Value}"));
        }

        private string BuildQuery(IDictionary<string, string> data)
        {
            return string.Join("&", data.OrderBy(x => x.Key)
                .Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
        }

        private string HmacSHA512(string key, string input)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }



        public string CreatePaymentUrl(string orderId, decimal amount, string ipAddress)
        {
            var vnp_Params = new SortedList<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", _vnpayConfig.TmnCode },
                { "vnp_Amount", ((int)(amount * 100)).ToString() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", ipAddress },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", "Thanh toán đơn hàng #" + orderId },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", _vnpayConfig.ReturnUrl },
                { "vnp_TxnRef", orderId }
            };

            string query = BuildQuery(vnp_Params);
            string signData = BuildDataToSign(vnp_Params);
            string vnp_SecureHash = HmacSHA512(_vnpayConfig.HashSecret, signData);

            return $"{_vnpayConfig.BaseUrl}?{query}&vnp_SecureHash={vnp_SecureHash}";
        }

        // 🧩 Xác thực callback từ VNPay
        public bool ValidateResponse(IDictionary<string, string> queryParams)
        {
            if (!queryParams.ContainsKey("vnp_SecureHash")) return false;

            string receivedHash = queryParams["vnp_SecureHash"];
            queryParams.Remove("vnp_SecureHash");

            string signData = BuildDataToSign(queryParams);
            string computedHash = HmacSHA512(_vnpayConfig.HashSecret, signData);

            return receivedHash.Equals(computedHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task SavePaymentLogAsync(string orderId, decimal amount, string status, string transactionId)
        {
            var log = new PaymentLogs
            {
                orderId = orderId,
                amount = amount,
                paymentMethod = "VNPay",
                status = status,
                transactionId = transactionId
            };

            await _paymentRepository.AddAsync(log);
        }
    }
}
