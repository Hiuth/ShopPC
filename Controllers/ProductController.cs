using Microsoft.AspNetCore.Mvc;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using System.ComponentModel.DataAnnotations;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create/{brandId}/{subCategoryId}")]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> createProduct(
            [FromRoute(Name="brandId")] string brandId,
            [FromRoute(Name="subCategoryId")] string subCategoryId,
            [FromForm(Name ="productName")] string productName,
            [FromForm(Name = "price")] decimal price,
            [FromForm(Name = "stockQuantity")] int stockQuantity,
            [FromForm(Name = "description")] string description,
            [FromForm(Name = "status")] string status,
            [FromForm(Name ="warrantyPeriod")] string warrantyPeriod,
            IFormFile thumbnail)
        {
            var request = new ProductRequest
            {
                productName = productName,
                price = price,
                stockQuantity = stockQuantity,
                description = description,
                status = status,
                warrantyPeriod = warrantyPeriod
            };

            var response = new ApiResponse<ProductResponse>()
            {
                Message = "Create product Successfully"
            };

            try
            {
                var proudct = await _productService.CreateProduct(brandId,subCategoryId,request,thumbnail);
                response.Result = proudct;
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

        [HttpPut("update/{productId}")]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> updateProduct(
            [FromRoute(Name="productId")] string productId,
            [FromForm(Name = "brandId")] string? brandId,
            [FromForm(Name = "subCategoryId")] string? subCategoryId,
            [FromForm(Name = "productName")] string? productName,
            [FromForm(Name = "price")] decimal? price,
            [FromForm(Name = "stockQuantity")] int? stockQuantity,
            [FromForm(Name = "description")] string? description,
            [FromForm(Name = "status")] string? status,
            [FromForm(Name = "warrantyPeriod")] string? warrantyPeriod,
            IFormFile? thumbnail)
        {
            var request = new ProductRequest
            {
                productName = productName ?? string.Empty,
                price = price ?? 0,
                stockQuantity = stockQuantity ?? 0,
                description = description ?? string.Empty,
                status = status ?? string.Empty,
                warrantyPeriod = warrantyPeriod ?? string.Empty
            };

            var response = new ApiResponse<ProductResponse>()
            {
              Message = "Update product successfully"
            };

            try
            {
                var product = await _productService.UpdateProduct(productId, brandId, subCategoryId, request, thumbnail);
                response.Result = product;
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

        [HttpGet("getAll")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getAllProduct(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Get all product successfully"
            };
            try 
            {
                var product = await _productService.GetAllProduct(pageNumber,pageSize);
                response.Result = product;
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

        [HttpGet("getBysubCategoryId/{subCategoryId}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getProductBySubCategoryId(
            [FromRoute(Name ="subCategoryId")] string subCategoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Get products by Sub category successfully"
            };
            try
            {
                var product = await _productService.GetProductsBySubCategoryId(subCategoryId,pageNumber,pageSize);
                response.Result = product;
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

        [HttpGet("getByBrandId/{brandId}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getProductByBrandId(
            [FromRoute(Name="brandId")] string brandId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Get products by brandId successfully"
            };

            try
            {
                var product = await _productService.GetProductsByBrandId(brandId,pageNumber,pageSize);
                response.Result = product;
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

        [HttpGet("searchProduct/{key}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> searchProduct(
            [FromRoute(Name="key")] string key,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Search product successfully"
            };
            try
            {
                var product = await _productService.SearchProducts(key, pageNumber, pageSize);
                response.Result = product;
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


        [HttpGet("getByPriceRange/{minPrice}/{maxPrice}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getByPriceRange(
            [FromRoute(Name = "minPrice")] decimal minPrice,
            [FromRoute(Name ="maxPrice")] decimal maxPrice,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = " Get product by price range successfully"
            };
            try
            {
                var product = await _productService.GetProductsByPriceRange(minPrice,maxPrice, pageNumber, pageSize);
                response.Result = product;
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
