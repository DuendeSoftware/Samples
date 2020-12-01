// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.Models;
using System.Collections.Generic;

namespace IdentityServerHost
{
    public static class Config
    {
        public static IEnumerable<ApiScope> Scopes =>
            new[]
            {
                // api1 specific scopes
                new ApiScope("api1.scope1"),
                new ApiScope("api1.scope2"),
                
                // api2 specific scopes
                new ApiScope("api2.scope1"),
                new ApiScope("api2.scope2"),
                
                // shared scope between resources
                new ApiScope("shared.scope"),

                // scopes with no resource association
                new ApiScope("scope2"),
                new ApiScope("scope3"),
                
                // parameterized scope
                new ApiScope("transaction"), 
            };
        
        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api1", "API #1")
                {
                    Scopes = { "api1.scope1", "api1.scope2", "shared.scope" }
                },
                
                new ApiResource("api2", "API #2")
                {
                    Scopes = { "api2.scope1", "api2.scope2", "shared.scope" },
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "resources.and.scopes",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    ClientClaimsPrefix = "",
                    
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "api1.scope1",
                        "api1.scope2",
                        
                        "api2.scope1",
                        "api2.scope2",
                        
                        "shared.scope",
                        
                        "scope2",
                        "scope3",
                        
                        "transaction"
                    }
                }
            };
    }
}