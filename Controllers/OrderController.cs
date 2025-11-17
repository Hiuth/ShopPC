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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        [Authorize(Roles ="ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> createOrder(
            [FromForm(Name ="status")] [Required] string status,
            [FromForm(Name ="totalAmount")] [Required] decimal totalAmount,
            [FromForm(Name ="customerName")] [Required] string cusotmerName,
            [FromForm(Name ="phoneNumber")] [Required] string phoneNumber,
            [FromForm(Name ="address")] [Required] string address)
        {
            var request = new OrderRequest
            {
                status =status,
                totalAmount = totalAmount,
                customerName =cusotmerName,
                phoneNumber = phoneNumber,
                address = address,
                isPaid = false
            };

            var response = new ApiResponse<OrderResponse>()
            {
                Message = "Create order Successfully"
            };
            try
            {
                var order = await _orderService.CreateOrder(request) ;
                response.Result = order;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpPut("update/{orderId}")]
        [Authorize(Roles ="ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> updateOrder(
            [FromRoute(Name = "orderId")] string orderId,
            [FromForm(Name = "status")] string? status,
            [FromForm(Name = "totalAmount")] decimal? totalAmount,
            [FromForm(Name = "customerName")] string? cusotmerName,
            [FromForm(Name = "phoneNumber")] string? phoneNumber,
            [FromForm(Name = "address")] string? address,
            [FromForm(Name ="isPaid")] bool? isPaid)
        {
            var request = new OrderRequest
            {
                status = status??string.Empty,
                totalAmount = totalAmount??0,
                customerName = cusotmerName??string.Empty,
                phoneNumber = phoneNumber ?? string.Empty,
                address = address ?? string.Empty,
                isPaid = isPaid??null
            };

            var response = new ApiResponse<OrderResponse>()
            {
                Message = "Update order Successfully"
            };
            try
            {
                var order = await _orderService.UpdateOrder(orderId, request);
                response.Result = order;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpGet("getOrderByAccountId")]
        [Authorize(Roles ="ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> getOrdersByAccountId()
        {
            var response = new ApiResponse<List<OrderResponse>>()
            {
                Message = "Get all orders by accountId successfully"
            };

            try
            {
                var order = await _orderService.GetOrdersByAccountId();
                response.Result = order;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpGet("getAllOrders")]
        [Authorize(Roles ="ADMIN")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> getAllOrders()
        {
            var response = new ApiResponse<List<OrderResponse>>()
            {
                Message = "Get all orders successfully"
            };
            try
            {
                var order = await _orderService.GetAllOrders();
                response.Result = order;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpDelete("deleteOrder/{orderId}")]
        [Authorize(Roles ="ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<string>>> deleteOrder(
            [FromRoute(Name = "orderId")] string orderId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete order Successfully"
            };
            try
            {
                var result = await _orderService.DeleteOrder(orderId);
                response.Result = result;
                return Ok(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return BadRequest(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return StatusCode(500, response);
            }
        }
    }
}
