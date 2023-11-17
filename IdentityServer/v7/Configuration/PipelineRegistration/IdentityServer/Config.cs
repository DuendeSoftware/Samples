﻿using Duende.IdentityServer.Models;

namespace IdentityServer;

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
            new ApiScope("IdentityServer.Configuration"),
            new ApiScope("SimpleApi")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "client",
                ClientName = "Client Credentials Client for DCR",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedScopes = { "IdentityServer.Configuration" }
            }

        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("configuration", "IdentityServer.Configuration API")
            {
                Scopes = { "IdentityServer.Configuration" },
                ApiSecrets = { new Secret("secret".Sha256()) }
            },
        };
}
