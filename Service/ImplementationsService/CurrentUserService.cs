using System.Security.Claims;
using ShopPC.Exceptions;
using ShopPC.Service.InterfaceService;

namespace ShopPC.Service.ImplementationsService
{
    public class CurrentUserService: ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            return userId ?? throw new AppException(ErrorCode.NOT_AUTHENTICATED);
        }
    }
}
