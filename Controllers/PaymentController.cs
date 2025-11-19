using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Models;
using ShopPC.Service.ImplementationsService;
using ShopPC.Service.InterfaceService;
using System.ComponentModel.DataAnnotations;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _config;
        public PaymentController(IPaymentService paymentService, IConfiguration config)
        {
            _paymentService = paymentService;
            _config = config;
        }

        [HttpGet("create")]
        [Authorize(Roles = "ADMIN,USER")]
        public ActionResult<ApiResponse<string>> CreatePayment(
             [FromQuery(Name = "orderId"), Required] string orderId,
             [FromQuery(Name = "amount"), Required] decimal amount)
        {
            var response = new ApiResponse<string>
            {
                Message = "Create payment URL successfully"
            };

            try
            {
               string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
 
                
                // Tạo URL thanh toán
                var paymentUrl = _paymentService.CreatePaymentUrl(orderId, amount, ipAddress);
                response.Result = paymentUrl;
                return Ok(response);
            }
            catch (AppException ex)
            {
                response.Code = 400;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = $"Internal server error: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        [HttpGet("vnpay-return")]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayReturn()
        {
            try
            {
                var queryParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

                // 1) Xác thực chữ ký
                if (!_paymentService.ValidateResponse(queryParams))
                {
                    // Chuyển hướng về FE với thông báo lỗi để UI hiển thị đẹp hơn
                    var clientBase = _config["Client:BaseUrl"] ?? "http://localhost:3000";
                    var original = HttpContext.Request.QueryString.Value ?? "";
                    var separator = string.IsNullOrEmpty(original) ? "?" : "&";
                    var targetUrl = $"{clientBase.TrimEnd('/')}/payment/return{original}{separator}error=invalid_signature";
                    return Redirect(targetUrl);
                }

                // 2) Lấy dữ liệu cần thiết
                string orderId = queryParams["vnp_TxnRef"];
                string transactionId = queryParams.GetValueOrDefault("vnp_TransactionNo", "");
                string responseCode = queryParams.GetValueOrDefault("vnp_ResponseCode", "");
                decimal amount = decimal.Parse(queryParams["vnp_Amount"]) / 100;

                // 3) Lưu log
                string status = responseCode == "00" ? "Success" : "Failed";
                await _paymentService.SavePaymentLogAsync(orderId, amount, status, transactionId);

                // 4) Redirect trình duyệt về FE /payment/return kèm nguyên query VNPay
                var clientBaseUrl = _config["Client:BaseUrl"] ?? "http://localhost:3000";
                var originalQuery = HttpContext.Request.QueryString.Value ?? "";
                var redirectUrl = $"{clientBaseUrl.TrimEnd('/')}/payment/return{originalQuery}";

                return Redirect(redirectUrl);
            }
            catch
            {
                // Có lỗi hệ thống: chuyển về FE với flag lỗi để UI xử lý
                var clientBase = _config["Client:BaseUrl"] ?? "http://localhost:3000";
                var redirectUrl = $"{clientBase.TrimEnd('/')}/payment/return?error=server_error";
                return Redirect(redirectUrl);
            }
        }

        [HttpGet("vnpay-ipn")]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayIpn()
        {
            try
            {
                var queryParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

                if (!_paymentService.ValidateResponse(queryParams))
                    return Content("INVALID");

                string orderId = queryParams["vnp_TxnRef"];
                string transactionId = queryParams.GetValueOrDefault("vnp_TransactionNo", "");
                string responseCode = queryParams.GetValueOrDefault("vnp_ResponseCode", "");
                decimal amount = decimal.Parse(queryParams["vnp_Amount"]) / 100;

                string status = responseCode == "00" ? "Success" : "Failed";

                // Lưu log IPN
                await _paymentService.SavePaymentLogAsync(orderId, amount, status, transactionId);

                return Content("OK"); // VNPay yêu cầu trả về "OK" nếu thành công
            }
            catch
            {
                return Content("ERROR");
            }
        }


    }
}
