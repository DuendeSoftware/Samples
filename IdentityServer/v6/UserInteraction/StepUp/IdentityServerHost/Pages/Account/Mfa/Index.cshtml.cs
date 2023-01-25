using System.Security.Claims;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.Mfa;

public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;

    public Index(IIdentityServerInteractionService interaction)
    {
        _interaction = interaction;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public void OnGetAsync(string returnUrl)
    {
        BuildModel(returnUrl);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var existingProps = (await HttpContext.AuthenticateAsync()).Properties;
            var claims = User.Claims
                .Append(new Claim(JwtClaimTypes.AuthenticationMethod, "mfa"))
                .Append(new Claim(JwtClaimTypes.AuthenticationContextClassReference, "1"));
            var newPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "mfa", "name", "role"));
            await HttpContext.SignInAsync(newPrincipal, existingProps);

            var authContext = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);
            if (authContext != null)
            {
                // Safe to trust input, because authContext is non-null
                return Redirect(Input.ReturnUrl);
            }
        }
        // something went wrong, show form with error
        BuildModel(Input.ReturnUrl);
        return Page();
    }

    public void BuildModel(string returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl
        };
    }
}
