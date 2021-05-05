// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Stores;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerHost.WsFed
{
    public class InMemoryWsFedProviderStore : OidcIdentityProviderStore, IIdentityProviderStore
    {
        private readonly IEnumerable<WsFedProvider> _providers;

        public InMemoryWsFedProviderStore(IEnumerable<WsFedProvider> providers, IConfigurationDbContext context, ILogger<OidcIdentityProviderStore> logger) : base(context, logger)
        {
            _providers = providers;
        }

        public new async Task<IdentityProvider> GetBySchemeAsync(string scheme)
        {
            var provider = await base.GetBySchemeAsync(scheme);
            
            if (provider == null)
            {
                provider = _providers.SingleOrDefault(x => x.Scheme == scheme);
            }

            return provider;
        }
    }
}
