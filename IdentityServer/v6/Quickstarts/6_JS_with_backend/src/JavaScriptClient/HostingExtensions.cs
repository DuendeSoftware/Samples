using System.Security.Claims;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authorization;

namespace JavaScriptClient;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();

        builder.Services.AddBff().AddRemoteApis();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5001";
                options.ClientId = "bff";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.Scope.Add("api1");
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
            });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();

        app.UseBff();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/local/identity", [Authorize] (ClaimsPrincipal user) =>
            {
                var name = user.FindFirst("name")?.Value ?? user.FindFirst("sub")?.Value;
                return Results.Json(new { message = "Local API Success!", user = name });
            }).AsBffApiEndpoint();

            endpoints.MapBffManagementEndpoints();

            endpoints.MapRemoteBffApiEndpoint("/remote", "https://localhost:6001")
                .RequireAccessToken(Duende.Bff.TokenType.User);
        });

        return app;
    }
}