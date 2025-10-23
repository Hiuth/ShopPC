using System.Collections.Concurrent;

namespace ShopPC.Service.ImplementationsService
{
    public class OtpService
    {
        private readonly ConcurrentDictionary<string, (string otp, DateTime expireAt)> _otpStorage = new();

        public string GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _otpStorage[email] = (otp, DateTime.UtcNow.AddMinutes(3));
            return otp;
        }

        public bool VerifyOtp(string email, string inputOtp)
        {
            if (_otpStorage.TryGetValue(email, out var data))
            {
                if (DateTime.UtcNow > data.expireAt)
                    return false;
                return data.otp == inputOtp;
            }
            return false;
        }
    }
}
