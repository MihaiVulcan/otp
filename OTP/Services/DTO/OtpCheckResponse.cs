namespace OTP.Services.DTO
{
    public class OtpCheckResponse
    {
        public bool Success { get; }

        public OtpCheckResponse(bool success)
        {
            Success = success;
        }
    }
}
