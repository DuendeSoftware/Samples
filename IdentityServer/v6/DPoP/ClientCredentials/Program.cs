using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Duende.AccessTokenManagement;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;
using IdentityModel;

namespace ClientCredentialsDPoPClient;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseSerilog()
                
            .ConfigureServices((services) =>
            {
                services.AddDistributedMemoryCache();

                services.AddClientCredentialsTokenManagement()
                    .AddClient("dpop", client =>
                    {
                        client.TokenEndpoint = "https://localhost:5001/connect/token";

                        client.ClientId = "dpop";
                        //client.ClientId = "dpop.nonce";
                        client.ClientSecret = "905e4892-7610-44cb-a122-6209b38c882f";

                        client.Scope = "scope1";
                        client.DPoPJsonWebKey = CreateDPoPKey();
                    });

                services.AddClientCredentialsHttpClient("client", "dpop", client =>
                {
                    client.BaseAddress = new Uri("https://localhost:5005/");
                });

                services.AddHostedService<DPoPClient>();
            });

        return host;
    }

    private static string CreateDPoPKey()
    {
        var key = new RsaSecurityKey(RSA.Create(2048));
        var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
        jwk.Alg = "PS256";
        var jwkJson = JsonSerializer.Serialize(jwk);
        return jwkJson;
    }

}