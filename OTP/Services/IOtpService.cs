using OTP.Services.DTO;

namespace OTP.Services
{
    public interface IOtpService
    {
        OtpGetResponse GetOtp(OtpGetRequest request);

        OtpCheckResponse CheckOtp(OtpCheckRequest request);
    }
}
