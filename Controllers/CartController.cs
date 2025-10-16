using Microsoft.AspNetCore.Mvc;
using ShopPC.Service.InterfaceService;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add/{accountId}/{productId}")]
        public async Task<ActionResult<ApiResponse<CartResponse>>> AddToCart(
            [FromRoute(Name = "accountId"), Required] string accountId,
            [FromRoute(Name = "productId"), Required] string productId,
            [FromForm(Name = "quantity")] int quantity)
        {
            var request = new CartRequest
            {
                quantity = quantity
            };

            var response = new ApiResponse<CartResponse>()
            {
                Message = "Add to cart successfully"
            };
            try
            {
                var cart = await _cartService.AddToCart(accountId, productId, request);
                response.Result = cart;
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

        [HttpPut("update/{cartId}")]
        public async Task<ActionResult<ApiResponse<CartResponse>>> UpdateCart(
            [FromRoute(Name = "cartId"), Required] string cartId,
            [FromForm(Name = "quantity")] int? quantity)
        {
            var request = new CartRequest
            {
                quantity = quantity ?? 0
            };
            var response = new ApiResponse<CartResponse>()
            {
                Message = "Update cart successfully"
            };
            try
            {
                var cart = await _cartService.UpdateCart(cartId, request);
                response.Result = cart;
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

        [HttpGet("get/{accountId}")]
        public async Task<ActionResult<ApiResponse<List<CartResponse>>>> GetCartByAccountId(
            [FromRoute(Name = "accountId"), Required] string accountId)
        {
            var response = new ApiResponse<List<CartResponse>>()
            {
                Message = "Get cart by accountId successfully"
            };
            try
            {
                var carts = await _cartService.GetCartByAccountId(accountId);
                response.Result = carts;
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

        [HttpDelete("clear/{cartId}")]
        public async Task<ActionResult<ApiResponse<string>>> ClearCart(
            [FromRoute(Name = "cartId"), Required] string cartId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Clear cart successfully"
            };
            try
            {
                var result = await _cartService.ClearCart(cartId);
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

        [HttpDelete("clearAll/{accountId}")]
        public async Task<ActionResult<ApiResponse<string>>> ClearAllCart(
            [FromRoute(Name = "accountId"), Required] string accountId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Clear all cart successfully"
            };
            try
            {
                var result = await _cartService.ClearAllCart(accountId);
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
