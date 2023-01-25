using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.ResponseHandling;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServerHost;

public class StepUpInteractionResponseGenerator : AuthorizeInteractionResponseGenerator
{
    public StepUpInteractionResponseGenerator(
        IdentityServerOptions options,
        ISystemClock clock,
        ILogger<AuthorizeInteractionResponseGenerator> logger,
        IConsentService consent,
        IProfileService profile) : base(options, clock, logger, consent, profile)
    {
    }

   protected override async Task<InteractionResponse> ProcessLoginAsync(ValidatedAuthorizeRequest request)
    {
        var result = await base.ProcessLoginAsync(request);

        if (!result.IsLogin && !result.IsError)
        {
            if(request.AuthenticationContextReferenceClasses.Contains("mfa") &&
                request.Subject.FindFirst(c => c.Type == "amr" && c.Value == "mfa") == null) 
            {
                result.RedirectUrl = "/Account/Mfa";
            }
        }
        return result;
    }
}
