namespace ShopPC.Service.InterfaceService
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);
        Task Logout(string token);

        Task<string> SendOtpForgotPassword();

        Task<string> ResetPassword(string otp, string newPassword);
    }
}
