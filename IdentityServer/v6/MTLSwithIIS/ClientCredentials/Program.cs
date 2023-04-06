using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Shared;

namespace Client
{
    public static class Urls
    {
        public const string IdentityServer = "https://identity.mtls.dev:5001";
        public const string ApiBaseMtls = "https://api.mtls.dev:6001";
        public const string ApiMtls = ApiBaseMtls + "/identity";
    }

    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Console MTLS Client";

            var response = await RequestTokenAsync();
            response.Show();

            await CallServiceAsync(response.AccessToken);

            Console.ReadKey();
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient(GetHandler());

            var disco = await client.GetDiscoveryDocumentAsync(Urls.IdentityServer);
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.MtlsEndpointAliases.TokenEndpoint,

                ClientId = "mtls",
                ClientCredentialStyle = ClientCredentialStyle.PostBody,
                Scope = "scope1"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task CallServiceAsync(string token)
        {
            var client = new HttpClient(GetHandler());
            client.SetBearerToken(token);


           

            var response = await client.GetAsync(Urls.ApiMtls);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            //"\n\nService claims:".ConsoleGreen();
            //Console.WriteLine(JsonSerializer.Serialize(JsonDocument.Parse(response), new JsonSerializerOptions { WriteIndented = true }));
        }

        static SocketsHttpHandler GetHandler()
        {
            var handler = new SocketsHttpHandler();

            // Any of these mechanisms of loading a certificate work

            // If you want to load certificates from the certificate store, they must be imported into the store
            // Also, you'll need to change the name or thumbprint here to match the certificate that you generate
            var cert = X509.CurrentUser.My.SubjectDistinguishedName.Find("CN=client.mtls.dev, OU=TITAN\\joede@Titan (Joe DeCock), O=mkcert development certificate").Single();
            // var cert = X509.CurrentUser.My.Thumbprint.Find("fc8968c0cc7ff70e53f7a8cfb48cd0d32902c6b0").Single();
            
            // If you want to distribute the client certificate with the client, you'll need to include the certificate in the built output
            // var cert = new X509Certificate2("client.mtls.dev-client.p12", "changeit");

            handler.SslOptions.ClientCertificates = new X509CertificateCollection { cert };

            return handler;
        }
    }
}
