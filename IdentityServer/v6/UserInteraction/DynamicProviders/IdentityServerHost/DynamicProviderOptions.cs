using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Hosting.DynamicProviders;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IdentityServerHost
{

    class CustomOidcConfigureOptions : ConfigureAuthenticationOptions<OpenIdConnectOptions, OidcProvider>
    {
        private readonly IdentityServerOptions _identityServerOptions;
        private readonly IServiceProvider _serviceProvider;

        public CustomOidcConfigureOptions(
            IHttpContextAccessor httpContextAccessor,
            IdentityServerOptions identityServerOptions,
            IServiceProvider serviceProvider
            ) : base(httpContextAccessor)
        {
            _identityServerOptions = identityServerOptions;
            _serviceProvider = serviceProvider;
        }

        protected override void Configure(ConfigureAuthenticationContext<OpenIdConnectOptions, OidcProvider> context)
        {
            var provider = context.IdentityProvider;
            var options = context.AuthenticationOptions;

            // options are configured here

            Debug.WriteLine("Configure was called");
        }
    }
}
