using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.IdentityModel.Tokens.Jwt;

[assembly: OwinStartup(typeof(WebForms.Startup))]

namespace WebForms
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "cookies",
                ExpireTimeSpan = TimeSpan.FromMinutes(10),
                SlidingExpiration = true
            });

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                SignInAsAuthenticationType = "cookies",

                Authority = "https://demo.duendesoftware.com",
                
                ClientId = "interactive.confidential.hybrid",
                ClientSecret = "secret",
                
                RedirectUri = "https://localhost:44306/",
                PostLogoutRedirectUri = "https://localhost:44306/",

                ResponseType = "code id_token",
                Scope = "openid profile",
                
                UseTokenLifetime = false,
                SaveTokens = true,
                RedeemCode = true,
            });

            app.UseStageMarker(PipelineStage.Authenticate);
        }
    }
}