using ShopPC.Repository.InterfaceRepository;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Exceptions;
using ShopPC.Mapper;
using System.Threading.Tasks;
using PagedList.Core;

namespace ShopPC.Service.ImplementationsService
{
    public class AccountService: IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public AccountService(IAccountRepository accountRepository, ICloudinaryService cloudinaryService)
        {
            _accountRepository = accountRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<AccountResponse> CreateAccount(AccountRequest request, IFormFile file)
        {
            if (!await _accountRepository.IsEmailUniqueAsync(request.email))
            {
                throw new AppException(ErrorCode.ACCOUNT_ALREADY_EXISTS);
            }
            var account = AccountMapper.toAccount(request);
            account.accountImg = await _cloudinaryService.UploadImageAsync(file);
            await _accountRepository.AddAsync(account);
            return AccountMapper.toAccountResponse(account);
        }

        public async Task<AccountResponse> UpdateAccount(string accountId, AccountRequest request, IFormFile? file)
        {
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }
            var account = await _accountRepository.GetByIdAsync(accountId)
                ?? throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);

            if (!String.IsNullOrWhiteSpace(request.userName))
            {
                account.userName = request.userName;
            }

            if (!String.IsNullOrWhiteSpace(request.gender))
            {
                account.gender = request.gender;
            }

            if (!String.IsNullOrWhiteSpace(request.address))
            {
                account.address = request.address;
            }

            //if()
            //{
            //    updatePassWord
            //}

            await _accountRepository.UpdateAsync(account);
            return AccountMapper.toAccountResponse(account);
        }

        public async Task<AccountResponse> GetAccountById(string accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId)
                ?? throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            return AccountMapper.toAccountResponse(account);
        }

        public async Task<List<AccountResponse>> GetAllAccount()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Select(AccountMapper.toAccountResponse).ToList();
        }
    }
}
