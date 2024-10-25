using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("identity")]
public class IdentityController : ControllerBase
{
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(ILogger<IdentityController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        _logger.LogInformation("claims: {claims}", claims);

        var scheme = GetAuthorizationScheme(Request);
        var proofToken = GetDPoPProofToken(Request);

        return new JsonResult(new { scheme, proofToken, claims });
    }
    
    public static string? GetAuthorizationScheme(HttpRequest request)
    {
        return request.Headers.Authorization.FirstOrDefault()?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)[0];
    }

    public static string? GetDPoPProofToken(HttpRequest request)
    {
        return request.Headers[OidcConstants.HttpHeaders.DPoP].FirstOrDefault();
    }
}