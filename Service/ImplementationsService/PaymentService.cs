using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShopPC.Configuration;
using ShopPC.Exceptions;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly EmailService _emailService;
        public PaymentService(IOptions<VnPayConfig> vnpayConfig, IPaymentRepository paymentRepository, 
            IOrderRepository orderRepository, EmailService emailService, IAccountRepository accountRepository)
        {
            _vnpayConfig = vnpayConfig.Value;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _emailService = emailService;
            _accountRepository = accountRepository;
        }

        private string BuildDataToSign(IDictionary<string, string> data)
        {
            var sortedParams = data.OrderBy(x => x.Key)
                .Where(x => !string.IsNullOrEmpty(x.Value));
            
            var hashData = "";
            int i = 0;
            foreach (var param in sortedParams)
            {
                if (i == 1)
                {
                    hashData += "&" + WebUtility.UrlEncode(param.Key) + "=" + WebUtility.UrlEncode(param.Value);
                }
                else
                {
                    hashData += WebUtility.UrlEncode(param.Key) + "=" + WebUtility.UrlEncode(param.Value);
                    i = 1;
                }
            }
            return hashData;
        }

        private string BuildQuery(IDictionary<string, string> data)
        {
            var sortedParams = data.OrderBy(x => x.Key)
                .Where(x => !string.IsNullOrEmpty(x.Value));
            
            return string.Join("&", sortedParams.Select(kv => 
                $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
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
                { "vnp_Amount", ((decimal)(amount * 100)).ToString() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", ipAddress },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", "Thanh toan don hang " + orderId },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", _vnpayConfig.ReturnUrl },
                { "vnp_TxnRef", orderId }
            };


            // 1. Tạo chuỗi để ký (theo cách VNPay docs)
            string signData = BuildDataToSign(vnp_Params);
            
            // 2. Tạo hash
            string vnp_SecureHash = HmacSHA512(_vnpayConfig.HashSecret, signData);
            
            // 3. Tạo query string cho URL (có URL encode)
            string query = BuildQuery(vnp_Params);
            
            // 4. Tạo URL cuối cùng
            string finalUrl = $"{_vnpayConfig.BaseUrl}?{query}&vnp_SecureHash={vnp_SecureHash}";
            
            return finalUrl;
        }

        public bool ValidateResponse(IDictionary<string, string> queryParams)
        {
            if (!queryParams.ContainsKey("vnp_SecureHash")) 
            {
                return false;
            }

            string receivedHash = queryParams["vnp_SecureHash"];
            
            var paramsForSign = new SortedDictionary<string, string>();
            foreach (var param in queryParams)
            {
                if (param.Key != "vnp_SecureHash" && param.Key != "vnp_SecureHashType")
                {
                    paramsForSign[param.Key] = param.Value;
                }
            }

            string signData = BuildDataToSign(paramsForSign);
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
            
            var accountId = await _orderRepository.GetAccountIdByOrderIdAsync(orderId)??
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);

            var user = await _accountRepository.GetAccountById(accountId);

            var notiUser = $@"
                <div style='font-family: Arial, Helvetica, sans-serif; background-color: #f4f8ff; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 24px; border-top: 5px solid #2d89ef; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                        <h2 style='color: #2d89ef; margin-top: 0;'>Thông báo từ Nexora</h2>

                        <p style='font-size: 16px; color: #333333;'>
                            <b>Đơn hàng của bạn đã được thanh toán thành công.</b>
                        </p>

                        <p style='font-size: 14px; color: #555555; line-height: 1.6;'>
                            Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi.
                        </p>

                        <p style='font-size: 14px; color: #555555; line-height: 1.6;'>
                            Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ bộ phận hỗ trợ của Nexora.
                        </p>

                        <hr style='border: none; border-top: 1px solid #e0e0e0; margin: 20px 0;'>

                        <p style='font-size: 12px; color: #888888; text-align: center;'>
                            © Nexora – Hệ thống thương mại điện tử
                        </p>
                    </div>
                </div>";


            var notiAdmin = $@"
                <div style='font-family: Arial, Helvetica, sans-serif; background-color: #f4f8ff; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 24px; border-top: 5px solid #2d89ef; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                        <h2 style='color: #2d89ef; margin-top: 0;'>Thông báo hệ thống Nexora</h2>

                        <p style='font-size: 16px; color: #333333;'>
                            <b>Bạn có đơn hàng mới.</b>
                        </p>

                        <p style='font-size: 14px; color: #555555; line-height: 1.6;'>
                            Vui lòng đăng nhập vào trang quản trị để kiểm tra và xử lý đơn hàng.
                        </p>

                        <hr style='border: none; border-top: 1px solid #e0e0e0; margin: 20px 0;'>

                        <p style='font-size: 12px; color: #888888; text-align: center;'>
                            © Nexora – Admin Notification System
                        </p>
                    </div>
                </div>";


            // Gửi email ngầm, không chờ kết quả để không làm gián đoạn luồng chính, làm kiến trúc event-driven 
            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendEmailAsync(user.email, "Thông báo thanh toán thành công", notiUser);
                    await _emailService.SendEmailAsync("nextaura2025@gmail.com", "Thông báo đơn hàng mới", notiAdmin);
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu cần (có thể thêm logging service sau)
                    Console.WriteLine($"Error sending payment notification emails: {ex.Message}");
                }
            });

        }

        public string BuildFrontendRedirectUrl(IDictionary<string, string> vnPayQueryParams, string clientBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(clientBaseUrl))
                throw new ArgumentException("clientBaseUrl không được rỗng.", nameof(clientBaseUrl));

            // Lọc các key bắt đầu bằng vnp_ (nếu muốn giữ nguyên 100%) – hoặc giữ tất cả nếu cần
            var pairs = vnPayQueryParams
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}");

            string queryString = string.Join("&", pairs);

            // Ghép thành URL cuối cùng
            return $"{clientBaseUrl.TrimEnd('/')}/payment/return?{queryString}";
        }
    }
}
