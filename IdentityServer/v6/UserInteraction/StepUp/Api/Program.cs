using Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication()
    .AddJwtBearer(opt =>
    {
        opt.Authority = "https://localhost:5001";
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidTypes = new [] { "at+jwt" }
        };
        opt.MapInboundClaims = false;
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("MaxAgeOneMinute", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddRequirements(new MaxAgeRequirement(TimeSpan.FromMinutes(1)));
    });
    opt.AddPolicy("MfaRequired", p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireClaim("amr", "mfa");
    });
    opt.AddPolicy("RecentMfa", p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireClaim("amr", "mfa");
        p.AddRequirements(new MaxAgeRequirement(TimeSpan.FromMinutes(1)));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, MaxAgeHandler>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, StepUpAuthorizationMiddlewareResultHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
