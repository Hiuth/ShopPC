using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.Exceptions;
using ShopPC.Repository.InterfaceRepository;
using ShopPC.Service.InterfaceService;
using BC = BCrypt.Net.BCrypt;

namespace ShopPC.Service.ImplementationsService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly TokenValidator _tokenValidator;
        private readonly EmailService _emailService;
        private readonly OtpService _otpService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;

        public AuthService(AppDbContext context, JwtService jwtService, TokenValidator tokenValidator,
            EmailService emailService, OtpService otpService, IAccountRepository accountRepository,ICurrentUserService currentUserService )
        {
            _context = context;
            _jwtService = jwtService;
            _tokenValidator = tokenValidator;
            _emailService = emailService;
            _otpService = otpService;
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;

        }

        public async Task<string> SendOtpForgotPassword()
        {
            var account = await _accountRepository.GetByIdAsync(_currentUserService.GetCurrentUserId()) ??
                throw new AppException(ErrorCode.NOT_AUTHENTICATED);
            var otp = _otpService.GenerateOtp(account.email);

            var body = $@"
                <!DOCTYPE html>
                <html lang='vi'>
                <head>
                  <meta charset='UTF-8'>
                  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                  <title>Mã OTP - Nexora</title>
                </head>
                <body style='margin: 0; padding: 0; background-color: #f5f7fa; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, ""Helvetica Neue"", Arial, sans-serif;'>
                  <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #f5f7fa; padding: 40px 0;'>
                    <tr>
                      <td align='center'>
                        <table width='600' cellpadding='0' cellspacing='0' style='background-color: #ffffff; border-radius: 12px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.08); overflow: hidden;'>
          
                          <!-- Header Section -->
                          <tr>
                            <td style='background-color: #4f46e5; padding: 40px 40px 35px; text-align: center;'>
                              <h1 style='margin: 0; color: #ffffff; font-size: 26px; font-weight: 600; line-height: 1.3;'>
                                Xác Thực Đổi Mật Khẩu
                              </h1>
                              <p style='margin: 15px 0 0; color: rgba(255, 255, 255, 0.9); font-size: 16px; font-weight: 400;'>
                                Xin chào <strong>{account.userName}</strong>
                              </p>
                            </td>
                          </tr>

                          <!-- Content Section -->
                          <tr>
                            <td style='padding: 35px 40px;'>
              
                              <!-- Thông báo -->
                              <div style='text-align: center; margin-bottom: 30px;'>
                                <p style='margin: 0 0 10px; font-size: 16px; font-weight: 500; color: #374151;'>
                                  Yêu cầu đổi mật khẩu cho tài khoản
                                </p>
                                <p style='margin: 0; font-size: 15px; color: #6b7280;'>
                                  <strong style='color: #4f46e5;'>{account.email}</strong>
                                </p>
                              </div>

                              <!-- OTP Box -->
                              <table width='100%' cellpadding='0' cellspacing='0' style='margin: 25px 0;'>
                                <tr>
                                  <td align='center'>
                                    <div style='background-color: #f8fafc; border: 2px solid #e2e8f0; border-radius: 10px; padding: 25px 30px; display: inline-block; min-width: 250px;'>
                                      <p style='margin: 0 0 12px; font-size: 12px; color: #64748b; font-weight: 500; text-transform: uppercase; letter-spacing: 1px;'>
                                        Mã Xác Thực OTP
                                      </p>
                                      <div style='font-family: ""Courier New"", Courier, monospace; font-size: 36px; font-weight: 700; color: #4f46e5; letter-spacing: 6px; text-align: center; margin: 12px 0;'>
                                        {otp}
                                      </div>
                                      <p style='margin: 12px 0 0; font-size: 12px; color: #94a3b8; font-weight: 400;'>
                                        Có hiệu lực trong <strong>5 phút</strong>
                                      </p>
                                    </div>
                                  </td>
                                </tr>
                              </table>

                              <!-- Hướng dẫn -->
                              <div style='background-color: #f1f5f9; border-left: 3px solid #4f46e5; padding: 18px 20px; margin: 25px 0; border-radius: 0 6px 6px 0;'>
                                <h3 style='margin: 0 0 10px; font-size: 14px; color: #374151; font-weight: 600;'>
                                  Hướng dẫn sử dụng:
                                </h3>
                                <ol style='margin: 0; padding-left: 18px; font-size: 13px; color: #64748b; line-height: 1.6;'>
                                  <li>Nhập mã OTP vào form xác thực</li>
                                  <li>Tạo mật khẩu mới</li>
                                  <li>Hoàn tất đổi mật khẩu</li>
                                </ol>
                              </div>

                              <!-- Cảnh báo bảo mật -->
                              <div style='background-color: #fef3cd; border: 1px solid #fbbf24; border-radius: 6px; padding: 16px 18px; margin: 25px 0;'>
                                <h3 style='margin: 0 0 8px; font-size: 14px; font-weight: 600; color: #92400e;'>
                                  Lưu ý bảo mật
                                </h3>
                                <ul style='margin: 0; padding-left: 18px; font-size: 13px; color: #78350f; line-height: 1.6;'>
                                  <li>Không chia sẻ mã OTP với bất kỳ ai</li>
                                  <li>Nhân viên không yêu cầu mã OTP qua điện thoại</li>
                                  <li>Bỏ qua email nếu bạn không thực hiện yêu cầu</li>
                                </ul>
                              </div>

                              <!-- Liên hệ hỗ trợ -->
                              <div style='text-align: center; padding: 15px 0; border-top: 1px solid #e2e8f0; margin-top: 25px;'>
                                <p style='margin: 0 0 5px; font-size: 13px; color: #64748b;'>
                                  Cần hỗ trợ? Liên hệ
                                </p>
                                <p style='margin: 0;'>
                                  <a href='mailto:nextaura2025@gmail.com' 
                                     style='color: #4f46e5; text-decoration: none; font-weight: 500; font-size: 14px;'>
                                    nextaura2025@gmail.com
                                  </a>
                                </p>
                              </div>

                            </td>
                          </tr>

                          <!-- Footer -->
                          <tr>
                            <td style='background-color: #f8fafc; border-top: 1px solid #e2e8f0; padding: 20px 40px; text-align: center;'>
                              <p style='margin: 0 0 4px; font-size: 13px; color: #64748b;'>
                                © 2025 <strong style='color: #374151;'>Nexora</strong> - ShopPC Platform
                              </p>
                              <p style='margin: 0; font-size: 11px; color: #94a3b8;'>
                                Email tự động - Không trả lời trực tiếp
                              </p>
                            </td>
                          </tr>

                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>";

            await _emailService.SendEmailAsync(account.email, "Mã OTP đổi mật khẩu - Nexora", body);
            return "Đã gửi OTP thành công.";
        }


        public async Task<string> ResetPassword(string otp, string newPassword)
        {
            var account = await _accountRepository.GetByIdAsync(_currentUserService.GetCurrentUserId()) ??
                 throw new AppException(ErrorCode.NOT_AUTHENTICATED);

            if (!_otpService.VerifyOtp(account.email, otp))
                throw new AppException(ErrorCode.INVALID_OTP);

            if(BC.Verify(newPassword, account.password))
                throw new AppException(ErrorCode.PASSWORD_CAN_NOT_LIKES_OLD_PASSWORD);

            account.password = BC.HashPassword(newPassword);
            await _accountRepository.UpdateAsync(account);
            return "Đổi mật khẩu thành công.";
        }



        public async Task<string> Login(string email, string password)
        {
            var account = await _context.Users.FirstOrDefaultAsync(u => u.email == email);

            if (account == null)
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }

            if (!BC.Verify(password, account.password))
            {
                throw new AppException(ErrorCode.INVALID_CREDENTIALS);
            }

            var roleName = account.roleName;
            if (string.IsNullOrEmpty(roleName))
            {
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }

            var token = _jwtService.GenerateToken(account, roleName);
            return token;
        }

        public async Task Logout(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new AppException(ErrorCode.TOKEN_INVALID_OR_EXPIRED);

            await _tokenValidator.InvalidateTokenAsync(token);
        }
    }
}
