using Aspire.Web;
using Aspire.Web.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddOpenIdConnect(opt =>
    {
        opt.Authority = "https://localhost:5001";
        opt.ClientId = "web";
        opt.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";

        opt.ResponseType = "code";
        opt.Scope.Add("weather");

        opt.SaveTokens = true;

        opt.GetClaimsFromUserInfoEndpoint = true;
        opt.MapInboundClaims = false;
    });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("https://apiservice"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();

app.UseAntiforgery();

app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.MapPost("/Logout", async ctx =>
{
    // Sign out local session
    await ctx.SignOutAsync();

    // Initiate remote signout
    var props = new AuthenticationProperties
    {
        RedirectUri = "/"
    };
    await ctx.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, props);
});

app.MapDefaultEndpoints();

app.Run();
