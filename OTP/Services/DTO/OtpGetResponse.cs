namespace OTP.Services.DTO
{
    public class OtpGetResponse
    {
        public string Code { get; }

        public DateTime Expiry { get; }

        public OtpGetResponse(string code, DateTime expiry)
        {
            Code = code;
            Expiry = expiry;
        }
    }
}