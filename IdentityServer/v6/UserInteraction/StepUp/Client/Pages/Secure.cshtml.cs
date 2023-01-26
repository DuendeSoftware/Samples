using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages;

[Authorize]
public class SecureModel : PageModel
{
    public SecureModel(ILogger<SecureModel> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _http = clientFactory.CreateClient("StepUp");    
    }

    private readonly ILogger<SecureModel> _logger;
    private readonly HttpClient _http;

    public string? ApiResponse { get; private set; }

    public async Task OnGet()
    {
        var response = await _http.GetAsync("neither");
        if (response.IsSuccessStatusCode)
        {
            ApiResponse = (await response.Content.ReadAsStringAsync())
                .PrettyPrintJson();
        }
    }
}

