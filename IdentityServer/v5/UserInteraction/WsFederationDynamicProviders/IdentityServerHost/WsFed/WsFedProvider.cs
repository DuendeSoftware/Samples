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

        public WsFedProvider(IdentityProvider other) : base(other)
        {
            if (other.Type != "wsfed") throw new System.Exception($"Invalid type '{other.Type}'");
        }

        public string MetadataAddress 
        {
            get => this["MetadataAddress"];
            set => this["MetadataAddress"] = value; 
        }
        public string RelyingPartyId 
        {
            get => this["RelyingPartyId"];
            set => this["RelyingPartyId"] = value;
        }
        public bool AllowIdpInitiated 
        {
            get => this["AllowIdpInitiated"] == "true";
            set => this["AllowIdpInitiated"] = value ? "true" : "false";
        }
    }
}
