namespace ShopPC.Service.InterfaceService
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> Login(string email, string password);
        
        Task Logout(string token);

        Task<string> SendOtpForgotPassword(string email);

        Task<string> ResetPassword(string otp, string newPassword,string email);

        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
    }
}
