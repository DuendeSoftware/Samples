// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityServerHost
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public static IConfiguration Configuration { get; private set; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/basics/resources
                options.EmitStaticAudienceClaim = true;

                // MTLS stuff
                options.MutualTls.Enabled = true;
                options.MutualTls.AlwaysEmitConfirmationClaim = true;
                options.MutualTls.ClientCertificateAuthenticationScheme = "mTLS";
            });

            builder.AddTestUsers(TestUsers.Users);
            builder.AddInMemoryClients(Clients.List);
            builder.AddInMemoryIdentityResources(Resources.Identity);
            builder.AddInMemoryApiScopes(Resources.ApiScopes);

            // this allows MTLS to be used as client authentication
            builder.AddMutualTlsSecretValidators();

            services.AddAuthentication().AddCertificate("mTLS", opt =>
            {
                opt.RevocationMode = X509RevocationMode.NoCheck;
                opt.Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = ctx =>
                    {
                        ctx.Principal = Principal.CreateFromCertificate(ctx.ClientCertificate, includeAllClaims: true);
                        ctx.Success();
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}