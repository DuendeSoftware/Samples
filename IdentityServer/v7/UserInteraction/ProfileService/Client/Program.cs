using Client;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IDiscoveryCache>(r =>
{
    var factory = r.GetRequiredService<IHttpClientFactory>();
    return new DiscoveryCache(Urls.IdentityServer, () => factory.CreateClient());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie(options =>
    {
        options.Cookie.Name = "mvcbasic";
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = Urls.IdentityServer;
        options.RequireHttpsMetadata = false;

        options.ClientId = "interactive.mvc.sample";
        options.ClientSecret = "secret";

        // code flow + PKCE (PKCE is turned on by default)
        options.ResponseType = "code";
        options.UsePkce = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("email");
        options.Scope.Add("scope1");
        options.Scope.Add("offline_access");

        // not mapped by default
        options.ClaimActions.MapJsonKey("picture", "picture");

        // keeps id_token smaller
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
        options.MapInboundClaims = false;
        options.DisableTelemetry = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute() .RequireAuthorization();

app.Run();