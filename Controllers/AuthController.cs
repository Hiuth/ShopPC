using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Service.InterfaceService;

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
    }
}
