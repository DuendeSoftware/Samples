using System.Text.Json;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{

    public class CallApiModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        public string Json = string.Empty;

        public async Task OnGet()
        {
            var accessToken = await HttpContext.GetUserAccessTokenAsync();
            var client = httpClientFactory.CreateClient();
            client.SetBearerToken(accessToken.AccessToken!);

            var content = await client.GetStringAsync("https://localhost:6001/identity");

            var parsed = JsonDocument.Parse(content);
            var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

            Json = formatted;
        }
    }
}
