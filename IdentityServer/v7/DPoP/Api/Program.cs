using Api;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

Console.Title = "API";

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddControllers();
builder.Services.AddCors();

// this API will accept any access token from the authority
builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = "https://localhost:5001";
        options.TokenValidationParameters.ValidateAudience = false;
        options.MapInboundClaims = false;

        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

// layers DPoP onto the "token" scheme above
builder.Services.ConfigureDPoPTokensForScheme("token");

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();