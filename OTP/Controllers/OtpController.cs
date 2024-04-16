using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OTP.Services;
using OTP.Services.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService ;

        public OtpController(IMemoryCache memoryCache) {
            _otpService = new OtpService(memoryCache);
        }

        // GET: api/<OtpController>
        [HttpGet("test")]
        public string Test()
        {
            return "ok";
        }

        // POST api/<OtpController>
        [HttpPost("getOtp")]
        public IActionResult GetOpt([FromBody] OtpGetRequest request)
        {
            try
            {
                var otpResponse = _otpService.getOtp(request);
                return Ok(otpResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("checkOtp")]
        public IActionResult CheckOtp([FromBody] OtpCheckRequest request)
        {
            try
            {
                var otpResponse = _otpService.checkOtp(request);
                return Ok(otpResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
