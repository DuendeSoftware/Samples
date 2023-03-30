// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.Models;
using System.Collections.Generic;
using Duende.IdentityServer;
using Microsoft.Extensions.Configuration;

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
                        //new Secret("fc8968c0cc7ff70e53f7a8cfb48cd0d32902c6b0")
                        //{
                        //    Type = IdentityServerConstants.SecretTypes.X509CertificateThumbprint
                        //},
                        new Secret("CN=client.mtls.dev, OU=TITAN\\joede@Titan (Joe DeCock), O=mkcert development certificate")
                        {
                            Type = IdentityServerConstants.SecretTypes.X509CertificateName
                        }
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