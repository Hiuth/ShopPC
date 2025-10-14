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
    public class BrandController: ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("create/{categoryId}")]
        public async Task<ActionResult<ApiResponse<BrandResponse>>> createBrand(
            [FromRoute(Name = "categoryId")] string categoryId,
            [FromForm(Name = "brandName")][Required] string brandName)
        {
            var request = new BrandRequest
            {
                brandName = brandName
            };
            var response = new ApiResponse<BrandResponse>()
            {
                Message = "Create brand successfully"
            };
            try
            {
                var brand = await _brandService.createBrand(categoryId,request);
                response.Result = brand;  // Gán kết quả vào response
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

        [HttpPut("update/{brandId}")]
        public async Task<ActionResult<ApiResponse<BrandResponse>>> updateBrand(
            [FromRoute(Name = "brandId")] string brandId,
            [FromForm(Name = "brandName")] string? brandName,
            [FromForm(Name = "categoryId")] string? categoryId)
        {
            var request = new BrandRequest
            {
                brandName = brandName ?? string.Empty
            };
            var response = new ApiResponse<BrandResponse>()
            {
                Message = "Update brand successfully"
            };
            try
            {
                var brand = await _brandService.updateBrand(brandId,categoryId,request);
                response.Result = brand;  // Gán kết quả vào response
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
        public async Task<ActionResult<ApiResponse<List<BrandResponse>>>> getAllBrand()
        {
            var response = new ApiResponse<List<BrandResponse>>()
            {
                Message = "Get all brand successfully"
            };
            try
            {
                var brands = await _brandService.getAllBrand();
                response.Result = brands;  // Gán kết quả vào response
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

        [HttpGet("getById/{brandId}")]
        public async Task<ActionResult<ApiResponse<BrandResponse>>> getBrandById(
            [FromRoute(Name = "brandId")] string brandId)
        {
            var response = new ApiResponse<BrandResponse>()
            {
                Message = "Get brand by id successfully"
            };
            try
            {
                var brand = await _brandService.getBrandById(brandId);
                response.Result = brand;  // Gán kết quả vào response
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
        public async Task<ActionResult<ApiResponse<List<BrandResponse>>>> getBrandByCategoryId(
            [FromRoute(Name = "categoryId")] string categoryId)
        {
            var response = new ApiResponse<List<BrandResponse>>()
            {
                Message = "Get brand by category id successfully"
            };
            try
            {
                var brands = await _brandService.getBrandByCategoryId(categoryId);
                response.Result = brands;  // Gán kết quả vào response
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
