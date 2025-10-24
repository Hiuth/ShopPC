using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Service.ImplementationsService;

namespace ShopPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
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
                response.Code = 400;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
                return StatusCode(500, response);
            }
        }

        /// Đăng xuất - Vô hiệu hóa token hiện tại
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<string>>> Logout()
        {
            var response = new ApiResponse<string> { Message = "Logout successfully" };

            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                await _authService.Logout(token);
                response.Result = "Token has been invalidated";
                return Ok(response);
            }
            catch (AppException ex)
            {
                response.Code = 400;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
                return StatusCode(500, response);
            }
        }
    }
}
