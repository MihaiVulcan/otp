using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OTP.Services;
using OTP.Services.DTO;
using OTP.Services.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OTP.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly ILogger<OtpController> _logger;
        private readonly IOtpService _otpService ;

        public OtpController(IMemoryCache memoryCache, ILogger<OtpController> logger, IConfiguration configuration) {
            _otpService = new OtpService(memoryCache, configuration);
            _logger = logger;
        }

        // POST api/<OtpController>
        [HttpPost("getOtp")]
        public IActionResult GetOtp([FromBody] OtpGetRequest request)
        {
            try
            {
                _logger.LogInformation("Get OTP request recived for " + request.Email);
                var otpResponse = _otpService.GetOtp(request);
                _logger.LogInformation("Get OTP response sent for " + request.Email);
                return Ok(otpResponse);
            }
            catch (EmailException ex)
            {
                _logger.LogError("Email Exception for " + request.Email);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("Invalid");
            }
        }

        [HttpPost("checkOtp")]
        public IActionResult CheckOtp([FromBody] OtpCheckRequest request)
        {
            try
            {
                _logger.LogInformation("Check OTP request recived for " + request.Email);
                var otpResponse = _otpService.CheckOtp(request);
                _logger.LogInformation("Check OTP response sent for " + request.Email);
                return Ok(otpResponse);
            }
            catch (EmailException ex)
            {
                _logger.LogError("Email Exception for " + request.Email);
                return BadRequest(ex.Message);
            }
            catch (OtpException ex)
            {
                _logger.LogError("Otp Exception for " + request.Email);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("invalid");
            }
        }
    }
}
