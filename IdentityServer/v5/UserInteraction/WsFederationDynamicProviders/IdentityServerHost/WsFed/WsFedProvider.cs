// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;

namespace IdentityServerHost.WsFed
{
    public class WsFedProvider : IdentityProvider
    {
        public WsFedProvider()
        {
            // this must match the value used in the call to AddProviderType
            Type = "wsfed";
        }

        public string MetadataAddress { get; set; }
        public string RelyingPartyId { get; set; }
    }
}
