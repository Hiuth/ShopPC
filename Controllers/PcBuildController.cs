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
    public class PcBuildController : ControllerBase
    {
        private readonly IPcBuildService _pcBuildService;
        public PcBuildController(IPcBuildService pcBuildService)
        {
            _pcBuildService = pcBuildService;
        }

        [HttpPost("create/{subCategoryId}")]
        public async Task<ActionResult<ApiResponse<PcBuildResponse>>> CreatePcBuild(
            [FromRoute(Name = "subCategoryId")] string subCategoryId,
            [FromForm(Name = "pcBuildName")][Required] string pcBuildName,
            [FromForm(Name = "description")] string description,
            [FromForm(Name = "price")] decimal price,
            [FromForm(Name = "status")] string status,
             IFormFile file)
        {
            var request = new PcBuildRequest
            {
                productName = pcBuildName,
                description = description,
                price = price,
                status = status
            };
            var response = new ApiResponse<PcBuildResponse>()
            {
                Message = "Create PC Build Successfully"
            };

            try
            {
                var pcBuild = await _pcBuildService.CreatePcBuild(subCategoryId, request, file);
                response.Result = pcBuild;
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

        [HttpPut("update/{pcBuildId}")]
        public async Task<ActionResult<ApiResponse<PcBuildResponse>>> UpdatePcBuild(
            [FromRoute(Name = "pcBuildId")] string pcBuildId,
            [FromForm(Name = "subCategoryId")] string? subCategoryId,
            [FromForm(Name = "pcBuildName")] string? pcBuildName,
            [FromForm(Name = "description")] string? description,
            [FromForm(Name = "price")] decimal? price,
            [FromForm(Name = "status")] string? status,
             IFormFile? file)
        {
            var request = new PcBuildRequest
            {
                productName = pcBuildName ?? string.Empty,
                description = description,
                price = price,
                status = status ?? string.Empty
            };
            var response = new ApiResponse<PcBuildResponse>()
            {
                Message = "Update PC Build Successfully"
            };
            try
            {
                var pcBuild = await _pcBuildService.UpdatePcBuild(pcBuildId, subCategoryId, request, file);
                response.Result = pcBuild;
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

        [HttpGet("getById/{pcBuildId}")]
        public async Task<ActionResult<ApiResponse<PcBuildResponse>>> GetPcBuildById(
            [FromRoute(Name = "pcBuildId")] string pcBuildId)
        {
            var response = new ApiResponse<PcBuildResponse>()
            {
                Message = "Get PC Build By Id Successfully"
            };
            try
            {
                var pcBuild = await _pcBuildService.GetPcBuildById(pcBuildId);
                response.Result = pcBuild;
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

        [HttpDelete("delete/{pcBuildId}")]
        public async Task<ActionResult<ApiResponse<string>>> DeletePcBuild(
            [FromRoute(Name = "pcBuildId")] string pcBuildId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete PC Build Successfully"
            };
            try
            {
                var result = await _pcBuildService.DeletePcBuild(pcBuildId);
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

        [HttpGet("getAll")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<PcBuildResponse>>>> GetAllPcBuilds(
            [FromQuery(Name = "pageNumber")] int pageNumber = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<PcBuildResponse>>()
            {
                Message = "Get All PC Builds Successfully"
            };
            try
            {
                var pcBuilds = await _pcBuildService.GetAllPcBuilds(pageNumber, pageSize);
                response.Result = pcBuilds;
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

        [HttpGet("getBySubCategory/{subCategoryId}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<PcBuildResponse>>>> GetPcBuildsBySubCategoryId(
            [FromRoute(Name = "subCategoryId")] string subCategoryId,
            [FromQuery(Name = "pageNumber")] int pageNumber = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 10)
        {
            var response = new ApiResponse<PaginatedResponse<PcBuildResponse>>()
            {
                Message = "Get PC Builds By SubCategory Successfully"
            };
            try
            {
                var pcBuilds = await _pcBuildService.GetPcBuildsBySubCategoryId(subCategoryId, pageNumber, pageSize);
                response.Result = pcBuilds;
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
