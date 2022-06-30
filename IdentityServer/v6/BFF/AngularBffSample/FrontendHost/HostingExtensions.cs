// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.Bff.Yarp;

namespace FrontendHost;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        // add BFF services.
        builder.Services.AddBff().AddRemoteApis();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";
            })
            .AddCookie("cookie", options =>
            {
                options.Cookie.Name = "__Host-bff";
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://demo.duendesoftware.com";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";

                options.ResponseType = "code";
                options.ResponseMode = "query";

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.MapInboundClaims = false;

                // request scopes
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("api");

                // and refresh token
                options.Scope.Add("offline_access");

                options.TokenValidationParameters = new()
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseRouting();

        // add CSRF protection and status code handling for API endpoints
        app.UseBff();
        app.UseAuthorization();

        // local API endpoints
        app.MapControllers().RequireAuthorization().AsBffApiEndpoint();

        app.MapBffManagementEndpoints();

        // enable proxying to remote API
        // app.MapRemoteBffApiEndpoint("/todos", "https://localhost:5020/todos").RequireAccessToken(Duende.Bff.TokenType.User);

        app.MapFallbackToFile("index.html");

        return app;
    }
}
