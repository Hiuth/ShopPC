using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.Exceptions;
using ShopPC.Service.InterfaceService;

namespace ShopPC.Service.ImplementationsService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly TokenValidator _tokenValidator;

        public AuthService(AppDbContext context, JwtService jwtService, TokenValidator tokenValidator)
        {
            _context = context;
            _jwtService = jwtService;
            _tokenValidator = tokenValidator;
        }

        public async Task<string> Login(string email, string password)
        {
            var account = await _context.Users
                .Include(u => u.roleName)
                .FirstOrDefaultAsync(u => u.email == email && u.password == password);
            if (account == null || account.password != password)
            {
                throw new AppException(ErrorCode.INVALID_CREDENTIALS);
            }
            var roles = new List<string> { account.roleName };
            var token = _jwtService.GenerateToken(account, roles);
            return token;
        }

        public async Task Logout(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new AppException(ErrorCode.TOKEN_INVALID_OR_EXPIRED);

            await _tokenValidator.InvalidateTokenAsync(token);

        }
    }
}
