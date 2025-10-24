using System.IdentityModel.Tokens.Jwt;
using ShopPC.Data;
using ShopPC.Models;

namespace ShopPC.Service.ImplementationsService
{
    public class TokenValidator
    {
        private readonly AppDbContext _context;

        public TokenValidator(AppDbContext context) { 
            _context = context;
        }

        public bool IsTokenInvalidated(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token); //Đọc token JWT từ chuỗi token đầu vào và phân tích nó thành một đối tượng JwtSecurityToken.
            var jti = jwt.Id; //Lấy giá trị JTI (JWT ID) từ token đã phân tích. JTI là một định danh duy nhất cho token.
            return _context.InvalidatedTokens.Any(t => t.id == jti); //Kiểm tra trong cơ sở dữ liệu xem có bất kỳ bản ghi nào trong bảng InvalidatedTokens có id trùng với JTI của token hay không.
        }

        public async Task InvalidateTokenAsync(string token)//Khi người dùng đăng xuất, hàm này sẽ được gọi để “hủy” token.
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var invalidatedToken = new InvalidatedToken
            {
                id = jwt.Id, // lấy id
                ExpiryTime = jwt.ValidTo// lấy thời gian hết hạn
            };

            _context.InvalidatedTokens.Add(invalidatedToken);
            await _context.SaveChangesAsync();
        }

    }
}
