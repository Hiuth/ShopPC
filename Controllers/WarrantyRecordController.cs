using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Service.ImplementationsService;
using ShopPC.Service.InterfaceService;
using System.ComponentModel.DataAnnotations;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarrantyRecordController : ControllerBase
    {
        private readonly IWarrantyRecordService _warrantyRecordService;
        public WarrantyRecordController(IWarrantyRecordService warrantyRecordService)
        {
            _warrantyRecordService = warrantyRecordService;
        }
        [HttpPost("create/{productId}/{orderId}/{productUnitId}")]
        [Authorize(Roles ="ADMIN")]
        public async Task<ActionResult<ApiResponse<WarrantyRecordResponse>>> CreateWarrantyPeriod(
            [FromRoute][Required] string productId,
            [FromRoute][Required] string orderId,
            [FromRoute][Required] string productUnitId)
        {
            var request = new WarrantyRecordRequest
            {
                status = "VALID"
            };

            var response = new ApiResponse<WarrantyRecordResponse>()
            {
                Message = "Create warranty period successfully"
            };

            try
            {
                var warranty = await _warrantyRecordService.CreateWarrantyPeriod(productId, orderId, productUnitId, request);
                response.Result = warranty;
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

        [HttpPut("update/{warrantyPeriodId}")]
        [Authorize(Roles = "ADMIN")]    
        public async Task<ActionResult<ApiResponse<WarrantyRecordResponse>>> UpdateWarrantyPeriod(
            [FromRoute][Required] string warrantyPeriodId,
            [FromForm][Required] string status)
        {
            var request = new WarrantyRecordRequest
            {
                status = status
            };
            var response = new ApiResponse<WarrantyRecordResponse>()
            {
                Message = "Update warranty period successfully"
            };
            try
            {
                var warranty = await _warrantyRecordService.UpdateWarrantyPeriod(warrantyPeriodId, request);
                response.Result = warranty;
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

        [HttpGet("GetBySerialNumber/{serialNumber}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<WarrantyRecordResponse>>> GetWarrantyRecordBySerialNumber(
            [FromRoute][Required] string serialNumber)
        {
            var response = new ApiResponse<WarrantyRecordResponse>()
            {
                Message = "Get warranty record by serial number successfully"
            };
            try
            {
                var warranty = await _warrantyRecordService.GetWarrantyRecordBySerialNumber(serialNumber);
                response.Result = warranty;
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

        [HttpGet("GetByImei/{imei}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<WarrantyRecordResponse>>> GetWarrnatyRecordByImei(
            [FromRoute][Required] string imei)
        {
            var response = new ApiResponse<WarrantyRecordResponse>()
            {
                Message = "Get warranty record by imei successfully"
            };
            try
            {
                var warranty = await _warrantyRecordService.GetWarrnatyRecordByImei(imei);
                response.Result = warranty;
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

        [HttpGet("GetByProductId/{productId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<List<WarrantyRecordResponse>>>> GetWarrantyRecordByProductId(
            [FromRoute][Required] string productId)
        {
            var response = new ApiResponse<List<WarrantyRecordResponse>>()
            {
                Message = "Get warranty records by product id successfully"
            };
            try
            {
                var warranties = await _warrantyRecordService.GetWarrantyRecordByProductId(productId);
                response.Result = warranties;
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

        [HttpGet("GetByOrderId/{orderId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<List<WarrantyRecordResponse>>>> GetWarrantyRecordByOrderId(
            [FromRoute][Required] string orderId)
        {
            var response = new ApiResponse<List<WarrantyRecordResponse>>()
            {
                Message = "Get warranty records by order id successfully"
            };
            try
            {
                var warranties = await _warrantyRecordService.GetWarrantyRecordByOrderId(orderId);
                response.Result = warranties;
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

        [HttpGet("GetByStatus/{status}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<List<WarrantyRecordResponse>>>> GetWarrantyRecordByStatus(
            [FromRoute][Required] string status)
        {
            var response = new ApiResponse<List<WarrantyRecordResponse>>()
            {
                Message = "Get warranty records by status successfully"
            };
            try
            {
                var warranties = await _warrantyRecordService.GetWarrantyRecordByStatus(status);
                response.Result = warranties;
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

        [HttpDelete("delete/{warrantyPeriodId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteWarrnatyRecordById(
            [FromRoute][Required] string warrantyPeriodId)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Delete warranty record successfully"
            };
            try
            {
                var result = await _warrantyRecordService.DeleteWarrnatyRecordById(warrantyPeriodId);
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
