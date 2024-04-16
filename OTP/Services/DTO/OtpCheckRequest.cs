namespace OTP.Services.DTO
{
    public class OtpCheckRequest
    {
        public string Email { get; }

        public string Code { get; }

        public OtpCheckRequest(string email, string code)
        {
            Email = email;
            Code = code;
        }
    }
}
