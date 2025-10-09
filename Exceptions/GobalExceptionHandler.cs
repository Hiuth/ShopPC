using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ShopPC.DTO.Response;

namespace ShopPC.Exceptions
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                await HandleAppExceptionAsync(context, ex);
            }
            catch (Exception)
            {
                await HandleUnhandledExceptionAsync(context);
            }
        }

        private static Task HandleAppExceptionAsync(HttpContext context, AppException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.ErrorCode.Status;

            var response = new ApiResponse<object>
            {
                Code = ex.ErrorCode.Code,
                Message = ex.ErrorCode.Message,
                Result = null
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleUnhandledExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiResponse<object>
            {
                Code = ErrorCode.INTERNAL_SERVER_ERROR.Code,
                Message = ErrorCode.INTERNAL_SERVER_ERROR.Message,
                Result = null
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
