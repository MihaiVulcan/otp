namespace OTP.Authentication
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IConfiguration _configuration;

        public ApiKeyAuthMiddleware(RequestDelegate requestDelegate, IConfiguration configuration)
        {
            _requestDelegate = requestDelegate;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Headers.TryGetValue(AuthConstans.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key missing");
                return;
            }

            var apiKey = _configuration.GetValue<string>(AuthConstans.ApiKeySectionName);
            if(apiKey is not null && !apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("InvalidApiKey");
                return;
            }

            await _requestDelegate(context);
        }
    }
}
