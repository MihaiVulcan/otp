namespace OTP.Services.Exceptions
{
    public class OtpException : Exception
    {
        public OtpException() { }

        public OtpException(string message) : base(message) { }

    }
}
