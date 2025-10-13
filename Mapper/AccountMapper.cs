using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.DTO.Request;
namespace ShopPC.Mapper
{
    public class AccountMapper
    {
        public static Account toAccount(AccountRequest accountRequest)
        {
            return new Account
            {
                userName = accountRequest.userName,
                password = accountRequest.password,
                gender = accountRequest.gender,
                email = accountRequest.email,
                phoneNumber = accountRequest.phoneNumber,
                address = accountRequest.address
            };
        }

        public static AccountResponse toAccountResponse(Account account)
        {
            return new AccountResponse
            {
                id = account.id,
                userName = account.userName,
                password = account.password,
                createdAt = account.createdAt,
                gender = account.gender,
                email = account.email,
                address = account.address,
                phoneNumber = account.phoneNumber
            };
        }
    }
}
