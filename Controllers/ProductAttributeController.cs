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
    public class ProductAttributeController: ControllerBase
    {
        private readonly IProductAttributeSerivce _IProductAttributeSerivce;
        public ProductAttributeController(IProductAttributeSerivce productAttributeSerivce)
        {
            _IProductAttributeSerivce = productAttributeSerivce;
        }

        [HttpPost("create/{attributeId}/{productId}")]
        public async Task<ActionResult<ApiResponse<ProductAttributeResponse>>> createProductAttribute(
            [FromRoute(Name="attributeId")] string attributeId,
            [FromRoute(Name="productId")] string productId,
            [FromForm(Name ="value")] string value)
        {
            var request = new ProductAttributeRequest
            {
                value = value
            };
            var response = new ApiResponse<ProductAttributeResponse>()
            {
                Message = "Create product attribute Successfully"
            };

            try
            {
                var proudctAttribute = await _IProductAttributeSerivce.CreateProductAttribute(attributeId, productId, request);
                response.Result = proudctAttribute;
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

        [HttpPut("update/{productAttributeId}")]
        public async Task<ActionResult<ApiResponse<ProductAttributeResponse>>> updateProductAttribute(
            [FromRoute(Name = "productAttributeId")] string productAttributeId,
            [FromForm(Name = "attributeId")] string? attributeId,
            [FromForm(Name = "value")] string? value)
        {
            var request = new ProductAttributeRequest
            {
                value = value ?? string.Empty
            };
            var response = new ApiResponse<ProductAttributeResponse>()
            {
                Message = "Update product attribute Successfully"
            };
            try
            {
                var proudctAttribute = await _IProductAttributeSerivce.UpdateProductAttribute(productAttributeId, attributeId, request);
                response.Result = proudctAttribute;
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

        [HttpGet("getByProductId/{productId}")]
        public async Task<ActionResult<ApiResponse<List<ProductAttributeResponse>>>> getProductAttributeByProductId(
            [FromRoute(Name = "productId")] string productId)
        {
            var response = new ApiResponse<List<ProductAttributeResponse>>()
            {
                Message = "Get product attribute by product id Successfully"
            };
            try
            {
                var proudctAttributes = await _IProductAttributeSerivce.GetProductAttributeByProductId(productId);
                response.Result = proudctAttributes;
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

        [HttpDelete("delete/{productAttributeId}")]
        public async Task<ActionResult<ApiResponse<string>>> deleteProductAttribute(
            [FromRoute(Name = "productAttributeId")] string productAttributeId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete product attribute Successfully"
            };
            try
            {
                var result = await _IProductAttributeSerivce.DeleteProductAttribute(productAttributeId);
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
