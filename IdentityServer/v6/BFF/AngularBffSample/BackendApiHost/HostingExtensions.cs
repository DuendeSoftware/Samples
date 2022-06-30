// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace BackendApiHost;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services
            .AddAuthentication("token")
            .AddJwtBearer("token", options =>
            {
                options.Authority = "https://demo.duendesoftware.com";
                options.Audience = "api";

                options.MapInboundClaims = false;
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiCaller", policy =>
            {
                policy.RequireClaim("scope", "api");
            });

            options.AddPolicy("RequireInteractiveUser", policy =>
            {
                policy.RequireClaim("sub");
            });
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers().RequireAuthorization("ApiCaller");

        return app;
    }
}
