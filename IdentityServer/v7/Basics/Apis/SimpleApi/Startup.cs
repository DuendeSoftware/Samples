﻿using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Client;
using Serilog;

namespace SimpleApi
{
    public class Startup
    {
        public Startup()
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        }
    
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog();

            services.AddControllers();

            // this API will accept any access token from the authority
            services.AddAuthentication("token")
                .AddJwtBearer("token", options =>
                {
                    options.Authority = Urls.IdentityServer;
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
}