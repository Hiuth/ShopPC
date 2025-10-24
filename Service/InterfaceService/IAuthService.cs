namespace ShopPC.Service.InterfaceService
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);
        Task Logout(string token);
    }
}
