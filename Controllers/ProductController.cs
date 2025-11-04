
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create/{categoryId}/{brandId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> createProduct(
            [FromRoute(Name = "categoryId")] string categoryId,
            [FromRoute(Name = "brandId")] string brandId,
            [FromForm(Name = "subCategoryId")] string? subCategoryId,
            [FromForm(Name = "productName")][Required] string productName,
            [FromForm(Name = "price")][Required] decimal price,
            [FromForm(Name = "stockQuantity")][Required] int stockQuantity,
            [FromForm(Name = "description")][Required] string description,
            [FromForm(Name = "status")][Required] string status,
            [FromForm(Name = "warrantyPeriod")][Required] int warrantyPeriod,
            [FromForm(Name ="isSerial")][Required] bool isSerial,
            IFormFile thumbnail)
        {
            var request = new ProductRequest
            {
                productName = productName,
                price = price,
                stockQuantity = stockQuantity,
                description = description,
                status = status,
                warrantyPeriod = warrantyPeriod,
                isSerial = isSerial,
            };

            var response = new ApiResponse<ProductResponse>()
            {
                Message = "Create product Successfully"
            };

            try
            {
                var proudct = await _productService.CreateProduct(brandId, categoryId, subCategoryId, request, thumbnail);
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> updateProduct(
            [FromRoute(Name = "productId")] string productId,
            [FromForm(Name = "brandId")] string? brandId,
            [FromForm(Name = "categoryId")] string? categoryId,
            [FromForm(Name = "subCategoryId")] string? subCategoryId,
            [FromForm(Name = "productName")] string? productName,
            [FromForm(Name = "price")] decimal? price,
            [FromForm(Name = "stockQuantity")] int? stockQuantity,
            [FromForm(Name = "description")] string? description,
            [FromForm(Name = "status")] string? status,
            [FromForm(Name = "warrantyPeriod")] int? warrantyPeriod,
            [FromForm(Name = "isSerial")] bool? isSerial,
            IFormFile? thumbnail)
        {
            var request = new ProductRequest
            {
                productName = productName ?? string.Empty,
                price = price,
                stockQuantity = stockQuantity,
                description = description ?? string.Empty,
                status = status ?? string.Empty,
                warrantyPeriod = warrantyPeriod,
                isSerial = isSerial??null,
               
            };

            var response = new ApiResponse<ProductResponse>()
            {
                Message = "Update product successfully"
            };

            try
            {
                var product = await _productService.UpdateProduct(productId, categoryId, brandId, subCategoryId, request, thumbnail);
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
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getAllProduct(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Get all product successfully"
            };
            try
            {
                var product = await _productService.GetAllProduct(pageNumber, pageSize);
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
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getProductBySubCategoryId(
            [FromRoute(Name = "subCategoryId")] string subCategoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Get products by Sub category successfully"
            };
            try
            {
                var product = await _productService.GetProductsBySubCategoryId(subCategoryId, pageNumber, pageSize);
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
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getProductByBrandId(
            [FromRoute(Name = "brandId")] string brandId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Get products by brandId successfully"
            };

            try
            {
                var product = await _productService.GetProductsByBrandId(brandId, pageNumber, pageSize);
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
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> searchProduct(
            [FromRoute(Name = "key")] string key,
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
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getByPriceRange(
            [FromRoute(Name = "minPrice")] decimal minPrice,
            [FromRoute(Name = "maxPrice")] decimal maxPrice,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = " Get product by price range successfully"
            };
            try
            {
                var product = await _productService.GetProductsByPriceRange(minPrice, maxPrice, pageNumber, pageSize);
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

        [HttpGet("getById/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> getProductById(
            [FromRoute(Name = "productId")] string productId)
        {
            var response = new ApiResponse<ProductResponse>()
            {
                Message = "Get product by id successfully"
            };
            try
            {
                var product = await _productService.GetProductById(productId);
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

        [HttpGet("getByCategoryId/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductResponse>>>> getProductByCategoryId(
            [FromRoute(Name = "categoryId")] string categoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<ProductResponse>>()
            {
                Message = "Get products by Category successfully"
            };
            try
            {
                var product = await _productService.GetProductByCategoryId(categoryId, pageNumber, pageSize);
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
