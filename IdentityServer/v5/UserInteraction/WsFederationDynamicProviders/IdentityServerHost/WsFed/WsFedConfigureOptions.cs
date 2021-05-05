// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Hosting.DynamicProviders;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Http;

namespace IdentityServerHost.WsFed
{
    class WsFedConfigureOptions : ConfigureAuthenticationOptions<WsFederationOptions, WsFedProvider>
    {
        public WsFedConfigureOptions(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void Configure(ConfigureAuthenticationContext<WsFederationOptions, WsFedProvider> context)
        {
            context.AuthenticationOptions.SignInScheme = context.DynamicProviderOptions.SignInScheme;
            context.AuthenticationOptions.SignOutScheme = context.DynamicProviderOptions.SignOutScheme;

            context.AuthenticationOptions.MetadataAddress = context.IdentityProvider.MetadataAddress;
            context.AuthenticationOptions.RequireHttpsMetadata = context.IdentityProvider.MetadataAddress.StartsWith("https");
            
            context.AuthenticationOptions.Wtrealm = context.IdentityProvider.RelyingPartyId;
            
            context.AuthenticationOptions.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
            context.AuthenticationOptions.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;
            
            context.AuthenticationOptions.CallbackPath = context.PathPrefix + "/signin";
            context.AuthenticationOptions.RemoteSignOutPath = context.PathPrefix + "/signout";
        }
    }
}
