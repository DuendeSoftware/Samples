using Microsoft.IdentityModel.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

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

                Authority = "https://localhost:5001/",

                ClientId = "interactive.webforms.sample",
                ClientSecret = "secret",

                RedirectUri = "https://localhost:44306/signin-oidc",
                PostLogoutRedirectUri = "https://localhost:44306/",

                ResponseType = "code",
                Scope = "openid profile offline_access",

                UseTokenLifetime = false,
                SaveTokens = true,
                RedeemCode = true,
                UsePkce = true,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    // Set the id_token_hint parameter during logout so that IdentityServer can safely redirect back
                    // here after logout. Unlike .NET Core, the Katana handler doesn't do this for us.
                    RedirectToIdentityProvider = async (msg) => {
                        if(msg.ProtocolMessage.PostLogoutRedirectUri != null)
                        {
                            var auth = await msg.OwinContext.Authentication.AuthenticateAsync("cookies");
                            if (auth.Properties.Dictionary.TryGetValue("id_token", out var idToken))
                            {
                                msg.ProtocolMessage.IdTokenHint = idToken;
                            }
                        }
                    }
                }
            });

            app.UseStageMarker(PipelineStage.Authenticate);
        }
    }
}