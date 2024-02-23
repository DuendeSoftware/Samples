using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("weather"),
        ];

    public static IEnumerable<Client> Clients =>
        [
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "web",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5014/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:5014/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:5014/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "weather" }
            },
        ];
}
