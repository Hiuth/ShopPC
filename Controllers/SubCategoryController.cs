using Microsoft.AspNetCore.Mvc;
using ShopPC.Service.InterfaceService;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;
        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpPost("create/{categoryId}")]
        public async Task<ActionResult<ApiResponse<SubCategoryResponse>>> createSubCategory(
            [FromForm(Name = "subCategoryName")] [Required] string subCategoryName,
            [FromRoute(Name = "categoryId")] string categoryId,
            [FromForm(Name ="description")][Required] string description,
           [Required] IFormFile file)
        {
            var request = new SubCategoryRequest
            {
                subCategoryName = subCategoryName,
                description = description
            };
            var response = new ApiResponse<SubCategoryResponse>()
            {
                Message = "Create subCategory successfully"
            };
            try
            {
                var subCategory = await _subCategoryService.createSubCategory(categoryId,request, file);
                response.Result = subCategory;  // Gán kết quả vào response
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

        [HttpPut("update/{subCategoryId}")]
        public async Task<ActionResult<ApiResponse<SubCategoryResponse>>> updateSubCategory(
            [FromRoute(Name = "subCategoryId")] string subCategoryId,
            [FromForm(Name = "categoryId")] string? categoryId,
            [FromForm(Name = "subCategoryName")] string? subCategoryName,
            [FromForm(Name = "description")] string? description,
            IFormFile? file)
        {
            var request = new SubCategoryRequest
            {
                subCategoryName = subCategoryName ??string.Empty,
                description = description??string.Empty
            };
            var response = new ApiResponse<SubCategoryResponse>()
            {
                Message = "Update subCategory successfully"
            };
            try
            {
                var subCategory = await _subCategoryService.updateSubCategory(subCategoryId, categoryId, request, file);
                response.Result = subCategory;  // Gán kết quả vào response
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
        public async Task<ActionResult<ApiResponse<List<SubCategoryResponse>>>> getAllSubCategory()
        {
            var response = new ApiResponse<List<SubCategoryResponse>>()
            {
                Message = "Get all subCategory successfully"
            };
            try
            {
                var subCategories = await _subCategoryService.getAllSubCategory();
                response.Result = subCategories;  // Gán kết quả vào response
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

        [HttpGet("getById/{subCategoryId}")]
        public async Task<ActionResult<ApiResponse<SubCategoryResponse>>> getSubCategoryById(
            [FromRoute(Name = "subCategoryId")] string subCategoryId)
        {
            var response = new ApiResponse<SubCategoryResponse>()
            {
                Message = "Get subCategory by id successfully"
            };
            try
            {
                var subCategory = await _subCategoryService.getSubCategoryById(subCategoryId);
                response.Result = subCategory;  // Gán kết quả vào response
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
        public async Task<ActionResult<ApiResponse<List<SubCategoryResponse>>>> getSubCategoryByCategoryId(
            [FromRoute(Name = "categoryId")] string categoryId)
        {
            var response = new ApiResponse<List<SubCategoryResponse>>()
            {
                Message = "Get subCategory by categoryId successfully"
            };
            try
            {
                var subCategories = await _subCategoryService.getSubCategoryByCategoryId(categoryId);
                response.Result = subCategories;  // Gán kết quả vào response
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
