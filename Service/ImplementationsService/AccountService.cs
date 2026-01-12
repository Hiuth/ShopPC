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
using BC = BCrypt.Net.BCrypt;

namespace ShopPC.Service.ImplementationsService
{
    public class AccountService: IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ICurrentUserService _currentUserService;
        private readonly EmailService _emailService;
        private readonly OtpService _otpService;

        public AccountService(IAccountRepository accountRepository, ICloudinaryService cloudinaryService, 
            EmailService emailService, OtpService otpService, ICurrentUserService currentUserService)
        {
            _accountRepository = accountRepository;
            _cloudinaryService = cloudinaryService;
            _emailService = emailService;
            _otpService = otpService;
            _currentUserService = currentUserService;
        }

        public async Task<string> SendOtpRegisterAsync(string email)
        {
            if (await _accountRepository.IsEmailUniqueAsync(email))
                throw new AppException(ErrorCode.ACCOUNT_ALREADY_EXISTS);

            var otp = _otpService.GenerateOtp(email);
            var body = $@"
            <div style='font-family: Arial;'>
                <h2>Chào mừng đến với Nexora!</h2>
                <p>Mã OTP xác thực đăng ký của bạn là:</p>
                <h1 style='color: #2d89ef;'>{otp}</h1>
                <p>Mã có hiệu lực trong <b>5 phút</b>.</p>
            </div>";

            await _emailService.SendEmailAsync(email, "Mã OTP đăng ký tài khoản", body);
            return "Đã gửi OTP thành công.";
        }



        public async Task<AccountResponse> CreateAccount(string otp, AccountRequest request)
        {
            if (!_otpService.VerifyOtp(request.email, otp))
                throw new AppException(ErrorCode.INVALID_OTP);


            var account = AccountMapper.toAccount(request);
            account.password = BC.HashPassword(request.password);
            account.accountImg = "https://res.cloudinary.com/dggt29zsn/image/upload/v1768190743/sd_ldoije.jpg";
            await _accountRepository.AddAsync(account);
            return AccountMapper.toAccountResponse(account);
        }

        public async Task<AccountResponse> UpdateAccount(AccountRequest request, IFormFile? file)
        {
            var accountId= _currentUserService.GetCurrentUserId();
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

            if(file != null)
            {
                if (!string.IsNullOrEmpty(account.accountImg))
                {
                    await _cloudinaryService.DeleteImageAsync(account.accountImg);
                    account.accountImg = await _cloudinaryService.UploadImageAsync(file);
                }
            }

            //if()
            //{
            //    updatePassWord
            //}

            await _accountRepository.UpdateAsync(account);
            return AccountMapper.toAccountResponse(account);
        }

        public async Task<AccountResponse> GetAccountById()
        {
            var accountId = _currentUserService.GetCurrentUserId();
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
