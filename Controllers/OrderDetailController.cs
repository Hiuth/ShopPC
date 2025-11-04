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
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        [HttpPost("createOrderDetail/{orderId}/{productId}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<OrderDetailResponse>>> createOrderDetail(
            [FromRoute(Name = "orderId")] string orderId,
            [FromRoute(Name = "productId")] string productId,
            [FromForm(Name = "quantity")][Required] int quantity,
            [FromForm(Name = "unitPrice")][Required] decimal unitPrice)
        {
            var request = new OrderDetailRequest
            {
                quantity = quantity,
                unitPrice = unitPrice
            };
            var response = new ApiResponse<OrderDetailResponse>()
            {
                Message = "Create order detail Successfully"
            };
            try
            {
                var orderDetail = await _orderDetailService.CreateOrderDetail(orderId, productId, request);
                response.Result = orderDetail;
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

        [HttpGet("getOrderDetails/{orderId}")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<ActionResult<ApiResponse<List<OrderDetailResponse>>>> getOrderDetailsByOrderId(
            [FromRoute(Name = "orderId")] string orderId)
        {
            var response = new ApiResponse<List<OrderDetailResponse>>()
            {
                Message = "Get order details by order id Successfully"
            };
            try
            {
                var orderDetails = await _orderDetailService.GetOrderDetailByOrderId(orderId);
                response.Result = orderDetails;
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

        [HttpDelete("deleteOrderDetails/{orderId}")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<ActionResult<ApiResponse<string>>> deleteOrderDetailsByOrderId(
            [FromRoute(Name = "orderId")] string orderId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete order details by order id Successfully"
            };
            try
            {
                var result = await _orderDetailService.DeleteOrderDetailbyOrderId(orderId);
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

        [HttpDelete("delete/{orderDetailId}")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<ActionResult<ApiResponse<string>>> deleteOrderDetailById(
            [FromRoute(Name = "orderDetailId")] string orderDetailId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete order detail by id Successfully"
            };
            try
            {
                var result = await _orderDetailService.DeleteOrderDetailbyId(orderDetailId);
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
