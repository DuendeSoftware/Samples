using System.Text.Json;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client;

[Authorize]
public class MaxAgeModel : PageModel
{
    public MaxAgeModel(IHttpClientFactory clientFactory)
    {
        _http = clientFactory.CreateClient("StepUp");    
    }

    private readonly HttpClient _http;

    public string? ApiResponse { get; private set; }

    public async Task OnGet()
    {
        var response = await _http.GetAsync("max-age");
        if (response.IsSuccessStatusCode)
        {
            ApiResponse = (await response.Content.ReadAsStringAsync())
                .PrettyPrintJson();
        } 
    }
}
