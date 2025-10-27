using Microsoft.AspNetCore.Mvc;
using ShopPC.Service.InterfaceService;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributesService _attributeService;
        public AttributeController(IAttributesService attributeService)
        {
            _attributeService = attributeService;
        }


        [HttpPost("create/{categoryId}")]
        [Authorize(Roles ="ADMIN")]
        public async Task<ActionResult<ApiResponse<AttributesResponse>>> CreateAttribute(
            [FromRoute(Name = "categoryId")] string categoryId,
            [FromForm(Name = "attributeName")][Required] string attributeName)
        {
            var request = new AttributesRequest
            {
                attributeName = attributeName
            };
            var response = new ApiResponse<AttributesResponse>()
            {
                Message = "Create attribute successfully"
            };
            try
            {
                var attribute = await _attributeService.CreateAttribute(categoryId, request);
                response.Result = attribute;  // Gán kết quả vào response
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

        [HttpPut("update/{attributeId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<AttributesResponse>>> UpdateAttribute(
            [FromRoute(Name = "attributeId")] string attributeId,
            [FromForm(Name = "attributeName")] string? attributeName,
            [FromForm(Name = "categoryId")] string? categoryId)
        {
            var request = new AttributesRequest
            {
                attributeName = attributeName ?? string.Empty
            };
            var response = new ApiResponse<AttributesResponse>()
            {
                Message = "Update attribute successfully"
            };
            try
            {
                var attribute = await _attributeService.UpdateAttribute(attributeId, categoryId, request);
                response.Result = attribute;  // Gán kết quả vào response
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

        [HttpGet("getById/{attributeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AttributesResponse>>> GetAttributeById(
            [FromRoute(Name = "attributeId")] string attributeId)
        {
            var response = new ApiResponse<AttributesResponse>()
            {
                Message = "Get attribute by id successfully"
            };
            try
            {
                var attribute = await _attributeService.GetAttributeById(attributeId);
                response.Result = attribute;  // Gán kết quả vào response
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<List<AttributesResponse>>>> GetAllAttributes()
        {
            var response = new ApiResponse<List<AttributesResponse>>()
            {
                Message = "Get all attributes successfully"
            };
            try
            {
                var attributes = await _attributeService.GetAllAttributes();
                response.Result = attributes;  // Gán kết quả vào response
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
        public async Task<ActionResult<ApiResponse<List<AttributesResponse>>>> GetAttributesByCategoryId(
            [FromRoute(Name = "categoryId")] string categoryId)
        {
            var response = new ApiResponse<List<AttributesResponse>>()
            {
                Message = "Get attributes by category id successfully"
            };
            try
            {
                var attributes = await _attributeService.GetAttributesByCategoryId(categoryId);
                response.Result = attributes;  // Gán kết quả vào response
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
