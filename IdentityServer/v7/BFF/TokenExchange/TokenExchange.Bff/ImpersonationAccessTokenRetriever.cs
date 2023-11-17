// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Duende.Bff;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace TokenExchange.Bff;

public class ImpersonationAccessTokenRetriever : DefaultAccessTokenRetriever
{
    public ImpersonationAccessTokenRetriever(ILogger<ImpersonationAccessTokenRetriever> logger) : base(logger)
    {
    }
    
    public override async Task<AccessTokenResult> GetAccessToken(AccessTokenRetrievalContext context)
    {
        var result = await base.GetAccessToken(context);

        if(result is BearerTokenResult bearerToken)
        {
            var client = new HttpClient();
            var exchangeResponse = await client.RequestTokenExchangeTokenAsync(new TokenExchangeTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                GrantType = OidcConstants.GrantTypes.TokenExchange,

                ClientId = "spa",
                ClientSecret = "secret",

                SubjectToken = bearerToken.AccessToken,
                SubjectTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken
            });
            if(exchangeResponse.IsError)
            {
                return new AccessTokenRetrievalError($"Token exchanged failed: {exchangeResponse.ErrorDescription}");
            }
            if(exchangeResponse.AccessToken is null)
            {
                return new AccessTokenRetrievalError("Token exchanged failed. Access token is null");
            } else
            {
                return new BearerTokenResult(exchangeResponse.AccessToken);
            }
        }

        return result;
    }
}
