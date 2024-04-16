namespace OTP.Models
{
    public class Otp
    {
        public string Code { get; }

        public DateTime Expiry { get; }

        public Otp(string code, DateTime expiry)
        {
            Code = code;
            Expiry = expiry;
        }
    }
}
