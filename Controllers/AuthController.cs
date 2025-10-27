using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Service.InterfaceService;
using System.ComponentModel.DataAnnotations;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromForm] string email, [FromForm] string password)
        {
            var response = new ApiResponse<string> { Message = "Login successfully" };
            try
            {
                var token = await _authService.Login(email, password);
                response.Result = token;
                return Ok(response);
            }
            catch (AppException ex)
            {
                response.Code = (int)ex.ErrorCode.Status;
                response.Message = ex.Message;
                return new ObjectResult(response) { StatusCode = (int)ex.ErrorCode.Status };
            }
            catch (Exception)
            {
                response.Code = 500;
                response.Message = "An unexpected internal server error occurred.";
                response.Result = null;
                return StatusCode(500, response);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> Logout()
        {
            var response = new ApiResponse<string> { Message = "Logout successfully" };
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authHeader.Substring("Bearer ".Length).Trim()
                    : authHeader;

                if (string.IsNullOrEmpty(token))
                {
                    throw new AppException(ErrorCode.TOKEN_INVALID_OR_EXPIRED);
                }

                await _authService.Logout(token);
                response.Result = "Token has been invalidated";
                return Ok(response);
            }
            catch (Exception)
            {
                response.Code = 500;
                response.Message = "An unexpected internal server error occurred.";
                return StatusCode(500, response);
            }
        }

        [HttpGet("send-otp-forgot-password")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<string>>> SendOtpForgotPassword()
        {
            var response = new ApiResponse<string>
            {
                Message = "Send OTP for forgot password successfully"
            };
            try
            {
                var result = await _authService.SendOtpForgotPassword();
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

        [HttpPut("reset-password")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<string>> resetPassword(
            [FromForm(Name="otp")][Required] string otp,
            [FromForm(Name ="newPassword")][Required] string newPassword)
        {
            var response = new ApiResponse<string>
            {
                Message = "Reset password successfully"
            };
            try
            {
                var result = await _authService.ResetPassword(otp,newPassword);
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
