namespace OTP.Services.DTO
{
    public class OtpGetRequest
    {
        public string Email { get; }

        public OtpGetRequest(string email)
        {
            Email = email;
        }
    }
}
