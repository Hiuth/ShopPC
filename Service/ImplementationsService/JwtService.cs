using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShopPC.Models;

namespace ShopPC.Service.ImplementationsService
{
    public class JwtService
    {
        private readonly IConfiguration _configuration; //IConfiguration được inject vào để lấy thông tin cấu hình từ file appsettings.json,
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Account user, string roles)
        {
            var claims = new List<Claim>//Claim là thông tin nhúng trong token, chứa dữ liệu của người dùng, Một token có thể chứa nhiều claim, mỗi claim là một cặp key - value.
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.email),//Địa chỉ email (định danh chính của user)
                new Claim("userName", user.userName), //Tên người dùng
                new Claim("userId", user.id), //ID của user trong hệ thống
                new Claim("Role", user.roleName), //Vai trò (role) chính của user
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Mã định danh duy nhất cho token này
            };

     
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)); //SymmetricSecurityKey: tạo khóa mã hóa dựa trên chuỗi “Key” trong cấu hình.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //SigningCredentials: Tạo chữ ký số cho token sử dụng thuật toán HMAC SHA256 và khóa mã hóa đã tạo.

            //Tiến hành tạo token JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], //Nguồn phát hành token (Nexora)   
                audience: _configuration["Jwt:Audience"], //Đối tượng sử dụng token (NexoraUser -> người dùng)
                claims: claims, //dữ liệu người dùng đã chuẩn bị ở trên
                expires: DateTime.UtcNow.AddSeconds(Convert.ToDouble(_configuration["Jwt:ValidDuration"])),// thời điểm hết hạn token
                signingCredentials: creds //chữ ký số  
             );

            return new JwtSecurityTokenHandler().WriteToken(token); //Trả về chuỗi token đã được mã hóa dưới dạng JWT
        }

        public JwtSecurityToken verifyToken(string token, bool isRefresh)
        {
            var tokenHander = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);// tạo khóa mã hóa từ cấu hình

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true
            };
            try
            {
                tokenHander.ValidateToken(token, parameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                if (isRefresh)
                {
                    var issuedAt = jwtToken.IssuedAt;
                    var refreshDuration = TimeSpan.FromSeconds(Convert.ToDouble(_configuration["Jwt:RefreshDuration"]));

                    if (issuedAt.Add(refreshDuration) < DateTime.UtcNow) {
                        throw new SecurityTokenException("Refresh token expired");
                    }
                }
                return jwtToken;

            } catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }
        }
    }
}
