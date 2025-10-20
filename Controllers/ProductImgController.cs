using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class ProductImgController : ControllerBase
    {
        private readonly IProductImgService _productImgService;
        public ProductImgController(IProductImgService productImgService)
        {
            _productImgService = productImgService;
        }

        [HttpPost("create/{productId}")]
        public async Task<ActionResult<ApiResponse<ProductImgResponse>>> createProductImg(
            [FromRoute(Name ="productId")]string productId,
           [Required] IFormFile file)
        {
            var response = new ApiResponse<ProductImgResponse>()
            {
                Message = " create product img successfully"
            };
            try
            {
                var productImg = await _productImgService.CreateProductImg(productId, file);
                response.Result = productImg;
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

        [HttpGet("getAllByProductId/{productId}")]
        public async Task<ActionResult<ApiResponse<List<ProductImgResponse>>>> getProductImgByProductId(
            [FromRoute(Name="productId")] string productId)
        {
            var response = new ApiResponse<List<ProductImgResponse>>()
            {
                Message = "Get all product img successfully"
            };
            try
            {
                var productImg = await _productImgService.GetProductImgByProductId(productId);
                response.Result = productImg;
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

        [HttpDelete("delete/{productImgId}")]
        public async Task<ActionResult<string>> deleteProductImg(
            [FromRoute(Name="productImgId")] string productImgId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete product img sucessfully"
            };
            try
            {
                var productImg = await _productImgService.DeleteProductImg(productImgId);
                response.Result = productImg;
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
