using Microsoft.Extensions.Caching.Memory;
using OTP.Models;
using OTP.Services.DTO;
using System.Security.Cryptography;

namespace OTP.Services
{
    public class OtpService : IOtpService
    {
        private static Dictionary<string, Otp> _otps = new Dictionary<string, Otp>() { };
        private readonly IMemoryCache _otpcache;
        private int _optLength = 6;
        private int _validTime = 60;

        public OtpService(IMemoryCache otpcache)
        {
            _otpcache = otpcache;
        }

        private string generateRandomNumber()
        {
            var min = (int)Math.Pow(10, _optLength - 1);
            var max = (int)Math.Pow(10, _optLength) - 1;
            return RandomNumberGenerator.GetInt32(min, max).ToString();
        }

        private Otp generateOtp(string email)
        {
            return new Otp(generateRandomNumber(), DateTime.UtcNow.AddSeconds(_validTime));
        }

        public OtpGetResponse getOtp(OtpGetRequest request)
        {
            if(request.Email == null || request.Email == "") 
            {
                throw new Exception("Invalid Email");
            }
            var otp = generateOtp(request.Email);
            _otpcache.Set(request.Email, otp, otp.Expiry);
            return new OtpGetResponse(otp.Code, otp.Expiry);
        }

        public OtpCheckResponse checkOtp(OtpCheckRequest request)
        {
            if (_otpcache.TryGetValue(request.Email, out _)){
                var otp = _otpcache.Get(request.Email) as Otp;
                if (otp.Code == request.Code)
                {
                    return new OtpCheckResponse(true);
                }
            }
            throw new Exception("OTP is not valid");
        }
    }
}