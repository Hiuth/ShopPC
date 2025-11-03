using ShopPC.Models;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface IAccountRepository: IGenericRepository<Account>
    {
        Task<bool> IsEmailUniqueAsync(string email);

        Task<List<Account>> searchAccountAsync(string key);

        Task<Account?> GetAccountById(string id);
    }
}
