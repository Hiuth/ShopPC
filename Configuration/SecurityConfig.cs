using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ShopPC.Configuration
{
    public static class SecurityConfig
    {
        public static void AddSecurityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");//configuration.GetSection("Jwt"): Lấy phần cấu hình "Jwt" trong file
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);//key: Là khóa bí mật (secret key) dùng để mã hóa và giải mã token JWT. ! là khẳng định không null

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //Khi có yêu cầu (request) gửi đến API, hệ thống mặc định sẽ kiểm tra token JWT. Nếu không có hoặc token sai → bị từ chối truy cập (401).
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; //RequireHttpsMetadata = false: Cho phép chạy ở môi trường HTTP (dành cho local dev).
                options.SaveToken = true;//SaveToken = true: Lưu token vào HttpContext để có thể truy cập sau.

                options.TokenValidationParameters = new TokenValidationParameters // xác thực token
                {
                    ValidateIssuer = true, //Có kiểm tra nguồn phát hành (Issuer) hay không.
                    ValidateAudience = true, // có đổi tượng sử dụng (Audience) hay không.
                    ValidateIssuerSigningKey = true, // có chữ ký bảo mật hay không
                    ValidateLifetime = true, // có kiểm tra thời gian sống của token hay không.
                    ValidIssuer = jwtSettings["Issuer"], // Issuer và audiece phải khớp với cấu hình trong token. 
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)// Khóa bí mật dùng để mã hóa và giải mã token JWT.
                    //Token nào không thỏa các điều kiện trên sẽ bị từ chối truy cập.
                };

                options.Events = new JwtBearerEvents 
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("""
                        { "code": 401, "message": "UNAUTHENTICATED", "result": null }
                        """);
                    }
                    //Khi client gửi request không có token hoặc token không hợp lệ, ASP.NET Core mặc định trả về lỗi HTML. lỗi 401
                };
            });

            services.AddAuthorization(); // kích hoạt cơ chế phân quyền

            services.AddCors(options => // cấu hình cors
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }
    }
}
