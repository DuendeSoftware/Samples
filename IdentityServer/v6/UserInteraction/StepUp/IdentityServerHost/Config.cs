using Duende.IdentityServer.Models;

namespace IdentityServerHost;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "step-up",
                ClientSecrets = { new Secret("secret".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:6001/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:6001/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },

                AllowedScopes = { "openid", "profile"}
            },
        };
}
