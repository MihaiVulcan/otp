using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using OTP.Services;
using OTP.Services.DTO;
using OTP.Models;
using OTP.Services.Exceptions;


namespace Opt.Tests.ServicesTests
{
    public class OtpServiceTests
    {
        private readonly IMemoryCache _mockedCache;

        private readonly IOtpService _otpService;

        public OtpServiceTests()
        {
            _mockedCache = Create.MockedMemoryCache();

            var inMemorySettings = new Dictionary<string, string> {
                {"OtpLength","6"},
                {"OtpValidTime", "10"},
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _otpService = new OtpService(_mockedCache, configuration);
        }

        [Fact]
        public void GenerateOtp_OKTest()
        {
            OtpGetResponse response = _otpService.GetOtp(new OtpGetRequest("testmail"));

            var otp = _mockedCache.Get("testmail") as Otp;

            Assert.NotNull(response);
            Assert.Equal(otp.Code, response.Code);
            Assert.Equal(6, response.Code.Length);
        }

        [Fact]
        public void GenerateOtp_NotOKTest()
        {
            Assert.Throws<EmailException>(() => _otpService.GetOtp(new OtpGetRequest("")));
        }

        [Fact]
        public void CheckOtp_OKTest()
        {
            OtpGetResponse getResponse = _otpService.GetOtp(new OtpGetRequest("testmail"));

            OtpCheckResponse checkResponse = _otpService.CheckOtp(new OtpCheckRequest("testmail", getResponse.Code));

            Assert.NotNull(checkResponse);
            Assert.True(checkResponse.Success);
        }

        [Fact]
        public void CheckOtp_NotOKEmailTest()
        {
            Assert.Throws<EmailException>(() => _otpService.CheckOtp(new OtpCheckRequest("", "123")));
        }

        [Fact]
        public void CheckOtp_NotOKOtpTest()
        {
            OtpGetResponse getResponse = _otpService.GetOtp(new OtpGetRequest("testmail"));

            Assert.Throws<OtpException>(() => _otpService.CheckOtp(new OtpCheckRequest("testmail", "123")));

        }

        [Fact]
        public void CheckOtpTwice_NotOKTest()
        {
            OtpGetResponse getResponse = _otpService.GetOtp(new OtpGetRequest("testmail"));

            OtpCheckResponse checkResponse = _otpService.CheckOtp(new OtpCheckRequest("testmail", getResponse.Code));

            Assert.NotNull(checkResponse);
            Assert.True(checkResponse.Success);

            Assert.Throws<OtpException>(() => _otpService.CheckOtp(new OtpCheckRequest("testmail", getResponse.Code)));
        }
    }
    
}
