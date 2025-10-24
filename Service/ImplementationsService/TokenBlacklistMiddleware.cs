using Microsoft.AspNetCore.Http;
using ShopPC.Service.ImplementationsService;

//Trong ASP.NET Core, middleware là các lớp nằm trong pipeline xử lý request.
//Request từ client sẽ đi qua từng middleware → nếu middleware nào dừng lại, request sẽ không được chuyển tiếp nữa.


namespace ShopPC.Service.ImplementationsService
{
    public class TokenBlacklistMiddleware
    {
        //Middleware này có nhiệm vụ: Kiểm tra xem token JWT trong request có bị “hủy” (blacklist) hay không.
        // Nếu token bị hủy, middleware sẽ trả về lỗi 401 Unauthorized.
        //nếu hợp lệ, request sẽ được di chuyển xuống các middleware tiếp theo hoặc controller xử lý.

        private readonly RequestDelegate _next; 
        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var tokenValidator = context.RequestServices.GetRequiredService<TokenValidator>();
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();//Lấy token JWT từ header Authorization của request.
            if (token != null)
            {
                if (tokenValidator.IsTokenInvalidated(token))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("""
                    { "code": 401, "message": "Token has been invalidated", "result": null }
                    """);
                    return;
                }
            }
            await _next(context); //Chuyển tiếp request đến middleware tiếp theo trong pipeline.
        }
    }
}
