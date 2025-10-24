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
    public class AccountController: ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("send-email")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> SendEmail(
            [FromQuery(Name = "Email"), Required] string email)
        {
            var response = new ApiResponse<string>()
            {
                Message = "Send email successfully"
            };
            try
            {
                await _accountService.SendOtpRegisterAsync(email);
                response.Result = "Email sent successfully";
                return new OkObjectResult(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return new BadRequestObjectResult(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }


        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AccountResponse>>> CreateAccount(
            [FromForm(Name ="userName"), Required ] string userName,
            [FromForm(Name = "email"), Required] string email,
            [FromForm(Name = "password"), Required] string password,
            [FromForm(Name = "gender"), Required] string gender,
            [FromForm(Name = "phoneNumber"), Required] string phoneNumber,
            [FromForm(Name = "address"), Required] string address,
            [FromForm(Name = "otp"), Required] string otp,
            [Required] IFormFile  file)
        {
            var request = new AccountRequest
            {
                userName = userName,
                email = email,
                password = password,
                gender = gender,
                phoneNumber = phoneNumber,
                address = address
            };
            var response = new ApiResponse<AccountResponse>()
            {
                Message = "Create account successfully"
            };
            try
            {
                var account = await _accountService.CreateAccount(otp,request, file);
                response.Result = account;
                return new OkObjectResult(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return new BadRequestObjectResult(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }


        [HttpPut("update/{accountId}")]
        [Authorize(Roles ="ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<AccountResponse>>> UpdateAccount(
            [FromRoute(Name = "accountId"), Required] string accountId,
            [FromForm(Name = "userName"), Required] string? userName,
            [FromForm(Name = "email"), Required] string? email,
            [FromForm(Name = "password"), Required] string? password,
            [FromForm(Name = "gender"), Required] string? gender,
            [FromForm(Name = "phoneNumber"), Required] string? phoneNumber,
            [FromForm(Name = "address"), Required] string? address,
            IFormFile? file)
        {
            var request = new AccountRequest
            {
                userName = userName??string.Empty,
                email = email ?? string.Empty,
                password = password ?? string.Empty,
                gender = gender ?? string.Empty,
                phoneNumber = phoneNumber ?? string.Empty,
                address = address ?? string.Empty
            };

            var response = new ApiResponse<AccountResponse>()
            {
                Message = "Update account successfully"
            };
            try
            {
                var account = await _accountService.UpdateAccount(accountId, request, file);
                response.Result = account;
                return new OkObjectResult(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return new BadRequestObjectResult(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpGet("getAccountById/{accountId}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<ApiResponse<AccountResponse>>> GetAccountById(
            [FromRoute(Name = "accountId"), Required] string accountId)
        {
            var response = new ApiResponse<AccountResponse>()
            {
                Message = "Get account by id successfully"
            };
            try
            {
                var account = await _accountService.GetAccountById(accountId);
                response.Result = account;
                return new OkObjectResult(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return new BadRequestObjectResult(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }

        [HttpGet("getAllAccount")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<List<AccountResponse>>>> GetAllAccount()
        {
            var response = new ApiResponse<List<AccountResponse>>()
            {
                Message = "Get all account successfully"
            };
            try
            {
                var accounts = await _accountService.GetAllAccount();
                response.Result = accounts;
                return new OkObjectResult(response);
            }
            catch (AppException ex)  // Catch AppException riêng
            {
                response.Code = 400;
                response.Message = ex.Message;
                response.Result = null;
                return new BadRequestObjectResult(response);
            }
            catch (Exception e)  // Catch Exception chung
            {
                response.Code = 500;
                response.Message = e.Message;
                response.Result = null;
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }
    }
}
