// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.Models;
using System.Collections.Generic;
using Duende.IdentityServer;

namespace IdentityServerHost
{
    public static class Clients
    {
        public static IEnumerable<Client> List =>
            new []
            {
                new Client
                {
                    ClientId = "mtls",

                    ClientSecrets =
                    {
                        new Secret("5D9E9B6B333CD42C99D1DE6175CC0F3EF99DDF68")
                        {
                            Type = IdentityServerConstants.SecretTypes.X509CertificateThumbprint
                        },
                    },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                    RedirectUris = { "https://localhost:44301/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44301/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44301/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope1" }
                },           
            };
    }
}