using System.Security.Cryptography;
using System.Text;

namespace Waybon.Api.Middleware
{
    public class SignatureVerificationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly string _publicKeyPem = configuration["Security:PublicKeyPem"] ?? throw new InvalidOperationException("Security:PublicKeyPem was not configured.");

        private static readonly TimeSpan MaxClockSkew = TimeSpan.FromMinutes(5);

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-Timestamp", out var timestampHeader) || !context.Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing signature headers.");
                return;
            }

            if (!long.TryParse(timestampHeader, out var timestampUnix))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid timestamp.");
                return;
            }

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(timestampUnix);
            if (Math.Abs((DateTimeOffset.UtcNow - requestTime).TotalSeconds) > MaxClockSkew.TotalSeconds)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Request timestamp is too old or in the future.");
                return;
            }

            context.Request.EnableBuffering();

            string body;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
            }
            context.Request.Body.Position = 0;

            var method = context.Request.Method;
            var pathAndQuery = context.Request.Path + context.Request.QueryString;
            var messageToVerify = $"{method}:{pathAndQuery}:{timestampHeader}:{body}";

            using var rsa = RSA.Create();
            rsa.ImportFromPem(_publicKeyPem);

            var signatureBytes = Convert.FromBase64String(signatureHeader!);
            var isValid = rsa.VerifyData
            (
                Encoding.UTF8.GetBytes(messageToVerify),
                signatureBytes,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid signature.");
                return;
            }

            await _next(context);
        }
    }
}