using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Api;

static class DPoPServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDPoPTokensForScheme(this IServiceCollection services, string scheme)
    {
        services.AddOptions<DPoPOptions>();

        services.AddTransient<DPoPJwtBearerEvents>();
        services.AddTransient<DPoPProofValidator>();
        services.AddDistributedMemoryCache();
        services.AddTransient<IReplayCache, DefaultReplayCache>();

        services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>>(new ConfigureJwtBearerOptions(scheme));
        

        return services;
    }

    public static IServiceCollection ConfigureDPoPTokensForScheme(this IServiceCollection services, string scheme, Action<DPoPOptions> configure)
    {
        services.Configure(scheme, configure);
        return services.ConfigureDPoPTokensForScheme(scheme);
    }
}
