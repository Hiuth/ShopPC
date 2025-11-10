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
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;

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
        public async Task<ActionResult<ApiResponse<string>>> VNPayReturn()
        {
            var response = new ApiResponse<string>
            {
                Message = "",
                Result = null,
                Code = 200
            };

            try
            {
                var queryParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

                // 1. Xác thực chữ ký
                if (!_paymentService.ValidateResponse(queryParams))
                {
                    response.Code = 400;
                    response.Message = "Chữ ký không hợp lệ!";
                    return BadRequest(response);
                }

                // 2. Lấy thông tin từ query
                string orderId = queryParams["vnp_TxnRef"];
                string transactionId = queryParams.GetValueOrDefault("vnp_TransactionNo", "");
                string responseCode = queryParams.GetValueOrDefault("vnp_ResponseCode", "");
                decimal amount = decimal.Parse(queryParams["vnp_Amount"]) / 100;

                // 3. Lưu log thanh toán
                string status = responseCode == "00" ? "Success" : "Failed";
                await _paymentService.SavePaymentLogAsync(orderId, amount, status, transactionId);

                response.Message = status == "Success" ? "Thanh toán thành công" : "Thanh toán thất bại";
                response.Result = orderId;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = $"Internal server error: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        [HttpGet("vnpay-ipn")]
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
