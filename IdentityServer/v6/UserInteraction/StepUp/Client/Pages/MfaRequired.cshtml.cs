using System.Text.Json;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client;

[Authorize]
public class MfaRequiredModel : PageModel
{
    public MfaRequiredModel(IHttpClientFactory clientFactory)
    {
        _http = clientFactory.CreateClient("StepUp");    
    }

    private readonly HttpClient _http;

    public string? ApiResponse { get; private set; }

    public async Task OnGet()
    {
        var response = await _http.GetAsync("mfa");

        if (response.IsSuccessStatusCode)
        {
            ApiResponse = (await response.Content.ReadAsStringAsync())
                .PrettyPrintJson();
        }
    }
}
