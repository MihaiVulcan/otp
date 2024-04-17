namespace OTP.Services.Exceptions
{
    public class EmailException : Exception
    {
        public EmailException() { }

        public EmailException(string message) : base(message) { }

    }
}
