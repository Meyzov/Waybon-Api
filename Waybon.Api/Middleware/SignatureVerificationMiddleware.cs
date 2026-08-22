using System.Security.Cryptography;
using System.Text;

namespace Waybon.Api.Middleware
{
    public class SignatureVerificationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly RSA _rsa = InitializeRsa(configuration["Security:PublicKeyPem"] ?? throw new InvalidOperationException("Security:PublicKeyPem was not configured."));

        private static readonly TimeSpan MaxClockSkew = TimeSpan.FromMinutes(5);

        private static RSA InitializeRsa(string pem)
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(pem);
            return rsa;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsPublicEndpoint(context.Request.Path))
            {
                await _next(context);
                return;
            }

            if (!TryValidateHeaders(context, out var timestampHeader, out var signatureHeader))
            {
                return;
            }

            if (!IsTimestampValid(timestampHeader))
            {
                await UnauthorizedResponse(context, "Request timestamp is too old or in the future.");
                return;
            }

            var body = await ReadRequestBodyAsync(context);
            if (!VerifySignature(context, timestampHeader, signatureHeader, body))
            {
                await UnauthorizedResponse(context, "Invalid signature.");
                return;
            }

            await _next(context);
        }

        // ==========================================
        // Helper Functions
        // ==========================================

        private static bool IsPublicEndpoint(PathString path)
        {
            return path.StartsWithSegments("/api/health") || path.StartsWithSegments("/api/metrics");
        }

        private static bool TryValidateHeaders(HttpContext context, out string timestamp, out string signature)
        {
            timestamp = context.Request.Headers["X-Timestamp"]!;
            signature = context.Request.Headers["X-Signature"]!;

            if (string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(signature))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.WriteAsync("Missing signature headers.").Wait();
                return false;
            }

            return true;
        }

        private static bool IsTimestampValid(string timestampHeader)
        {
            if (!long.TryParse(timestampHeader, out var timestampUnix))
            {
                return false;
            }

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(timestampUnix);
            return Math.Abs((DateTimeOffset.UtcNow - requestTime).TotalSeconds) <= MaxClockSkew.TotalSeconds;
        }

        private static async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;
            return body;
        }

        private bool VerifySignature(HttpContext context, string timestampHeader, string signatureHeader, string body)
        {
            var method = context.Request.Method;
            var pathAndQuery = context.Request.Path + context.Request.QueryString;
            var messageToVerify = $"{method}:{pathAndQuery}:{timestampHeader}:{body}";

            var signatureBytes = Convert.FromBase64String(signatureHeader);

            return _rsa.VerifyData
            (
                Encoding.UTF8.GetBytes(messageToVerify),
                signatureBytes,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );
        }

        private static Task UnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return context.Response.WriteAsync(message);
        }
    }
}