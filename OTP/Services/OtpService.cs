using Microsoft.Extensions.Caching.Memory;
using OTP.Models;
using OTP.Services.DTO;
using OTP.Services.Exceptions;
using System.Security.Cryptography;

namespace OTP.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _otpcache;
        private readonly int _optLength = 6;
        private readonly int _validTime = 60;
        private readonly int _min;
        private readonly int _max;

        public OtpService(IMemoryCache otpcache, IConfiguration configuration)
        {
            _otpcache = otpcache;
            _optLength = configuration.GetValue<int>("OtpLength");
            _validTime = configuration.GetValue<int>("OtpValidTime");
            _min = (int)Math.Pow(10, _optLength - 1);
            _max = (int)Math.Pow(10, _optLength) - 1;
        }

        private string generateRandomNumber()
        {

            return RandomNumberGenerator.GetInt32(_min, _max).ToString();
        }

        private Otp generateOtp()
        {
            return new Otp(generateRandomNumber(), DateTime.UtcNow.AddSeconds(_validTime));
        }

        public OtpGetResponse GetOtp(OtpGetRequest request)
        {
            
            if(string.IsNullOrEmpty(request.Email))
            {
                throw new EmailException("Invalid Email");
            }
            var otp = generateOtp();
            _otpcache.Set(request.Email, otp, otp.Expiry);
            return new OtpGetResponse(otp.Code, otp.Expiry);
        }

        public OtpCheckResponse CheckOtp(OtpCheckRequest request) //litera mare CheckOtp
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                throw new EmailException("Invalid Email");
            }

            if (_otpcache.TryGetValue(request.Email, out Otp? otp)){
                if (otp is not null && otp.Code == request.Code)
                {
                    _otpcache.Remove(request.Email);
                    return new OtpCheckResponse(true);
                }
            }

            throw new OtpException("OTP is not valid");
        }
    }
}