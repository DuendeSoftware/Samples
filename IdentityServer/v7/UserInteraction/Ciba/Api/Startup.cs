﻿using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace Api;

public class Startup
{
    public Startup()
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // this API will accept any access token from the authority
        services.AddAuthentication("token")
            .AddJwtBearer("token", options =>
            {
                options.Authority = Constants.Authority;
                options.TokenValidationParameters.ValidateAudience = false;
                
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireAuthorization();
        });
    }
}