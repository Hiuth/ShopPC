using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PcBuildItemController : ControllerBase
    {
        private readonly IPcBuildItemService _pcBuildItemService;
        public PcBuildItemController(IPcBuildItemService pcBuildItemService)
        {
            _pcBuildItemService = pcBuildItemService;
        }

        [HttpPost("create/{pcBuildId}/{productId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<PcBuildItemResponse>>> createPCBuildItem(
            [FromRoute(Name = "pcBuildId")] string pcBuildId,
            [FromRoute(Name = "productId")][Required] string productId,
            [FromForm(Name = "quantity")][Required] int quantity
            )
        {
            var request = new PcBuildItemRequest()
            {
                quantity = quantity
            };

            var response = new ApiResponse<PcBuildItemResponse>()
            {
                Message = "Create PC Build Item Successfully"
            };

            try
            {
                var pcBuildItem = await _pcBuildItemService.CreatePcBuildItem(pcBuildId, productId, request);
                response.Result = pcBuildItem;
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

        [HttpPut("update/{pcBuildItemId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<PcBuildItemResponse>>> updatePcBuildItem(
            [FromRoute(Name = "pcBuildItemId")] string pcBuildItemId,
            [FromForm(Name = "productId")] string? productId,
            [FromForm(Name = "quantity")] int? quantity
            )
        {
            var request = new PcBuildItemRequest()
            {
                quantity = quantity ?? 0
            };

            var response = new ApiResponse<PcBuildItemResponse>()
            {
                Message = "Update PC Build Item Successfully"
            };

            try
            {
                var pcBuildItem = await _pcBuildItemService.UpdatePcBuildItem(pcBuildItemId, productId, request);
                response.Result = pcBuildItem;
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

        [HttpGet("getAllByPcBuildById/{pcBuildItemId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<PcBuildItemResponse>>>> getAllPcBuildItem(
               [FromRoute(Name = "pcBuildItemId")] string pcBuildItemId)
        {
            var response = new ApiResponse<List<PcBuildItemResponse>>()
            {
                Message = "Get all PC Build Item Successfully"
            };

            try
            {
                var pcBuildItem = await _pcBuildItemService.GetPcBuildItemsByPcBuildId(pcBuildItemId);
                response.Result = pcBuildItem;
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

        [HttpDelete("delete/{pcBuildItemId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<string>>> deletePcBuildItem(
            [FromRoute(Name = "pcBuildItemId")] string pcBuildItemId)
        {
            var response = new ApiResponse<string>()
            {
                Message = ""
            };
            try
            {
                var pcBuildItem = await _pcBuildItemService.DeletePcBuildItem(pcBuildItemId);
                response.Result = pcBuildItem;
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
