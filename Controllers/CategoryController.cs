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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        { 
            _categoryService = categoryService;
        }


        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> createCategory(
            [FromForm(Name ="categoryName")] string categoryName, IFormFile file)
        {
            var request = new CategoryRequest
            {
                categoryName = categoryName
            };
            var response = new ApiResponse<CategoryResponse>()
            {
                Message = "Create category successfully"
            };
            try
            {
                var category = await _categoryService.createCategory(request, file);
                response.Result = category;  // Gán kết quả vào response
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

        [HttpPut("update/{categoryId}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> updateCategory(
            [FromRoute(Name ="categoryId")] string categoryId, [FromForm(Name ="categoryName")] string? categoryName, IFormFile? file)
        {
            var request = new CategoryRequest
            {
                categoryName = categoryName ?? string.Empty
            };
            var response = new ApiResponse<CategoryResponse>()
            {
                Message = "Update category successfully"
            };
            try
            {
                var category = await _categoryService.updateCategory(categoryId, request, file);
                response.Result = category;  // Gán kết quả vào response
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
        public async Task<ActionResult<ApiResponse<List<CategoryResponse>>>> getAllCategory()
        {
            var response = new ApiResponse<List<CategoryResponse>>()
            {
                Message = "Get all categories successfully"
            };
            try
            {
                var categories = await _categoryService.getAllCategory();
                response.Result = categories;  // Gán kết quả vào response
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

        [HttpGet("getById/{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> getCategoryById([FromRoute] string id)
        {
            var response = new ApiResponse<CategoryResponse>()
            {
                Message = "Get category by id successfully"
            };
            try
            {
                var category = await _categoryService.getCategoryById(id);
                response.Result = category;  // Gán kết quả vào response
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
