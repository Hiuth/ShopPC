using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IAccountService
    {
        Task<AccountResponse> CreateAccount(AccountRequest request,IFormFile file);

        Task<AccountResponse> UpdateAccount(string accountId, AccountRequest request, IFormFile? file);

        Task<AccountResponse> GetAccountById(string accountId);

        Task<List<AccountResponse>> GetAllAccount();

        //Task<string> deleteAccount(string accountId);

        
    }
}
