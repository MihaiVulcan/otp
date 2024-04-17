using MemoryCache.Testing.Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OTP.Controllers;
using OTP.Models;
using OTP.Services.DTO;

namespace Opt.Tests.ControllersTests
{
    public class OtpControllerTests
    {
        private readonly IMemoryCache _mockedCache;

        private readonly OtpController _controller;

        public OtpControllerTests()
        {
            _mockedCache = Create.MockedMemoryCache();

            var mockIlogger = new Mock<ILogger<OtpController>>();
            ILogger<OtpController> logger = mockIlogger.Object;

            var inMemorySettings = new Dictionary<string, string> {
                {"OtpLength","6"},
                {"OtpValidTime", "10"},
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new OtpController(_mockedCache, logger, configuration);
        }

        [Fact]
        public void OtpGet_OKTest()
        {
            IActionResult result = _controller.GetOtp(new OtpGetRequest("testmail"));
            var okResult = result as ObjectResult;

            Assert.NotNull(okResult);
            Assert.True(okResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public void OtpGet_NotOKTest()
        {
            IActionResult result = _controller.GetOtp(new OtpGetRequest(""));
            var okResult = result as ObjectResult;

            Assert.NotNull(okResult);
            Assert.True(okResult is BadRequestObjectResult);
        }

        [Fact]
        public void CheckOtp_OKTest()
        {
            IActionResult getRes = _controller.GetOtp(new OtpGetRequest("testmail"));
            var otp = _mockedCache.Get("testmail") as Otp;
            IActionResult checkRes = _controller.CheckOtp(new OtpCheckRequest("testmail", otp.Code));

            Assert.NotNull(checkRes);
            Assert.True(checkRes is OkObjectResult);
        }

        [Fact]
        public void CheckOtp_NotOKTest()
        {
            IActionResult getRes = _controller.GetOtp(new OtpGetRequest("testmail"));
            var otp = _mockedCache.Get("testmail") as Otp;
            IActionResult checkRes = _controller.CheckOtp(new OtpCheckRequest("testmail", "34"));

            Assert.NotNull(checkRes);
            Assert.True(checkRes is BadRequestObjectResult);
        }
    }
}