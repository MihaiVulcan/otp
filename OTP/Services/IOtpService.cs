using OTP.Services.DTO;

namespace OTP.Services
{
    public interface IOtpService
    {
        OtpGetResponse getOtp(OtpGetRequest request);

        OtpCheckResponse checkOtp(OtpCheckRequest request);
    }
}
