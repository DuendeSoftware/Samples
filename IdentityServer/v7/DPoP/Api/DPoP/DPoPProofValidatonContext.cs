using System.Security.Claims;

namespace Api;

public class DPoPProofValidatonContext
{
    /// <summary>
    /// The ASP.NET Core authentication scheme triggering the validation
    /// </summary>
    public required string Scheme { get; set; }

    /// <summary>
    /// The HTTP URL to validate
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// The HTTP method to validate
    /// </summary>
    public required string Method { get; set; }

    /// <summary>
    /// The DPoP proof token to validate
    /// </summary>
    public required string ProofToken { get; set; }

    /// <summary>
    /// The access token
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// The claims associated with the access token.
    /// </summary>
    public IEnumerable<Claim> AccessTokenClaims { get; set; } = Enumerable.Empty<Claim>();

}
