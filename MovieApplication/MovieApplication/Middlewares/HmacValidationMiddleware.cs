using Infrastructure.Services.Hmac;

namespace MovieApplication.Middlewares
{
    public class HmacValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HmacService _hmacService;
        private static readonly string[] _exemptPaths = ["/swagger", "/api/login"];

        public HmacValidationMiddleware(RequestDelegate next, HmacService hmacService)
        {
            _next = next;
            _hmacService = hmacService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            if (_exemptPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }
            var signature = context.Request.Headers["X-Signature"].FirstOrDefault();
            var timestamp = context.Request.Headers["X-Timestamp"].FirstOrDefault();
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("HMAC headers eksik.");
                return;
            }
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            var isValid = _hmacService.ValidateSignature(
                context.Request.Method,
                context.Request.Path,
                body,
                timestamp,
                signature);
            if (!isValid)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Geçersiz HMAC imzası.");
                return;
            }
            await _next(context);
        }
    }
}
