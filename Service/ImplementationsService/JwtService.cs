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

        public string GenerateAccessToken(Account user)
        {
            var claims = new List<Claim>//Claim là thông tin nhúng trong token, chứa dữ liệu của người dùng, Một token có thể chứa nhiều claim, mỗi claim là một cặp key - value.
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.email),//Địa chỉ email (định danh chính của user)
                new Claim("userName", user.userName), //Tên người dùng
                new Claim("userId", user.id), //ID của user trong hệ thống
                new Claim("Role", user.roleName), //Vai trò (role) chính của user
                new Claim("avatar", user.accountImg), //Ảnh đại diện của user)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Mã định danh duy nhất cho token này
                new Claim("type","access")
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

        public JwtSecurityToken verifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);// tạo khóa mã hóa từ cấu hình

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.FromMinutes(10),
                ValidateLifetime =  false
            };
            tokenHandler.ValidateToken(token, parameters, out var validatedToken);
            return (JwtSecurityToken)validatedToken;
        }

        public string GenerateRefreshToken(Account user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.email),//Địa chỉ email (định danh chính của user)
                new Claim("userName", user.userName), //Tên người dùng
                new Claim("userId", user.id), //ID của user trong hệ thống
                new Claim("Role", user.roleName), //Vai trò (role) chính của user
                new Claim("avatar", user.accountImg), //Ảnh đại diện của user)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("type","refresh")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var refreshToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], //Nguồn phát hành token (Nexora)   
                audience: _configuration["Jwt:Audience"], //Đối tượng sử dụng token (NexoraUser -> người dùng)
                claims: claims, //dữ liệu người dùng đã chuẩn bị ở trên
                expires: DateTime.UtcNow.AddSeconds(Convert.ToDouble(_configuration["Jwt:RefreshableDurationss"])),// thời điểm hết hạn token
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }
    }
}
