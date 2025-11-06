using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IAccountService
    {
        Task<string> SendOtpRegisterAsync(string email);

        Task<AccountResponse> CreateAccount(string otp,AccountRequest request);

        Task<AccountResponse> UpdateAccount(AccountRequest request, IFormFile? file);

        Task<AccountResponse> GetAccountById();

        Task<List<AccountResponse>> GetAllAccount();

        //Task<string> deleteAccount(string accountId);

        
    }
}
