using Microsoft.AspNetCore.Http.HttpResults;
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
    public class ProductUnitController : ControllerBase
    {
        private readonly IProductUnitService _productUnitService;
        public ProductUnitController(IProductUnitService productUnitService)
        {
            _productUnitService = productUnitService;
        }

        [HttpPost("create/{productId}")]
        public async Task<ActionResult<ApiResponse<ProductUnitResponse>>> createProductUnit(
            [FromRoute(Name = "productId")] string productId,
            [FromForm(Name = "imei")] string? imei,
            [FromForm(Name = "serialNumber")] string? serialNumber,
            [FromForm(Name = "status")] string status)
        {
            var request = new ProductUnitRequest
            {
                imei = imei,
                serialNumber = serialNumber,
                status = status
            };

            var response = new ApiResponse<ProductUnitResponse>
            {
                Message = "Product unit created successfully",
            };

            try
            {
                var proudctUnit = await _productUnitService.CreateProductUnit(productId, request);
                response.Result = proudctUnit;
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

        [HttpPut("update/{productUnitId}")]
        public async Task<ActionResult<ApiResponse<ProductUnitResponse>>> updateProductUnit(
            [FromRoute(Name = "productUnitId")] string productUnitId,
            [FromForm(Name = "imei")] string? imei,
            [FromForm(Name = "serialNumber")] string? serialNumber,
            [FromForm(Name = "status")] string? status)
        {
            var request = new ProductUnitRequest
            {
                imei = imei,
                serialNumber = serialNumber,
                status = status ?? string.Empty
            };
            var response = new ApiResponse<ProductUnitResponse>
            {
                Message = "Product unit updated successfully",
            };
            try
            {
                var proudctUnit = await _productUnitService.UpdateProductUnit(productUnitId, request);
                response.Result = proudctUnit;
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

        [HttpDelete("delete/{productUnitId}")]
        public async Task<ActionResult<ApiResponse<string>>> deleteProductUnit(
            [FromRoute(Name = "productUnitId")] string productUnitId)
        {
            var response = new ApiResponse<string>
            {
                Message = "Delete product unit successfully",
            };
            try
            {
                var result = await _productUnitService.DeleteProductUnit(productUnitId);
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

        [HttpGet("getProductUnitByProductId/{productId}")]
        public async Task<ActionResult<ApiResponse<List<ProductUnitResponse>>>> getProductUnitsByProductId(
            [FromRoute(Name = "productId")] string productId)
        {
            var response = new ApiResponse<List<ProductUnitResponse>>
            {
                Message = "Get product units by product id successfully",
            };
            try
            {
                var productUnits = await _productUnitService.GetProductUnitsByProductId(productId);
                response.Result = productUnits;
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

        [HttpGet("getProductUnitById/{productUnitId}")]
        public async Task<ActionResult<ApiResponse<ProductUnitResponse>>> getProductUnitById(
            [FromRoute(Name = "productUnitId")] string productUnitId)
        {
            var response = new ApiResponse<ProductUnitResponse>
            {
                Message = "Get product unit by id successfully",
            };
            try
            {
                var productUnit = await _productUnitService.GetProductUnitById(productUnitId);
                response.Result = productUnit;
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
