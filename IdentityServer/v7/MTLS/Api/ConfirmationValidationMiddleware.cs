using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace SampleApi
{
    public static class ConfirmationValidationExtensions
    {
        public static IApplicationBuilder UseConfirmationValidation(this IApplicationBuilder app, ConfirmationValidationMiddlewareOptions options = default)
        {
            return app.UseMiddleware<ConfirmationValidationMiddleware>(options ?? new ConfirmationValidationMiddlewareOptions());
        }
    }

    public class ConfirmationValidationMiddlewareOptions
    {
        public string JwtBearerSchemeName { get; set; } = JwtBearerDefaults.AuthenticationScheme;
    }
    
    // this middleware validate the cnf claim (if present) against the thumbprint of the X.509 client certificate for the current client
    public class ConfirmationValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ConfirmationValidationMiddlewareOptions _options;

        public ConfirmationValidationMiddleware(RequestDelegate next, ILogger<ConfirmationValidationMiddlewareOptions> logger, ConfirmationValidationMiddlewareOptions options = null)
        {
            _next = next;
            _logger = logger;
            _options ??= new ConfirmationValidationMiddlewareOptions();
        }

        public async Task Invoke(HttpContext ctx)
        {
            if (ctx.User.Identity.IsAuthenticated)
            {
                var cnfJson = ctx.User.FindFirst("cnf")?.Value;
                if (!String.IsNullOrWhiteSpace(cnfJson))
                {
                    var certificate = await ctx.Connection.GetClientCertificateAsync();
                    var thumbprint = Base64UrlTextEncoder.Encode(certificate.GetCertHash(HashAlgorithmName.SHA256));
                    
                    var cnf = JObject.Parse(cnfJson);
                    var sha256 = cnf.Value<string>("x5t#S256");

                    if (String.IsNullOrWhiteSpace(sha256) ||
                        !thumbprint.Equals(sha256, StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogError("certificate thumbprint does not match cnf claim.");
                        await ctx.ChallengeAsync(_options.JwtBearerSchemeName);
                        return;
                    }
                    
                    _logger.LogDebug("certificate thumbprint matches cnf claim.");
                }
            }

            await _next(ctx);
        }
    }
}