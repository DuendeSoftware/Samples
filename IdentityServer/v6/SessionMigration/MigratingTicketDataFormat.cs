using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SessionMigration;
public class MigratingTicketDataFormat : ISecureDataFormat<AuthenticationTicket>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ISecureDataFormat<AuthenticationTicket> inner;
    private CookieAuthenticationOptions options;
    private readonly string scheme;

    // Copied from Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler,
    // unfortunatley it's private and cannot be referenced.
    private const string SessionIdClaim = "Microsoft.AspNetCore.Authentication.Cookies-SessionId";

    public MigratingTicketDataFormat(
        IHttpContextAccessor httpContextAccessor,
        CookieAuthenticationOptions options,
        string scheme)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.options = options;
        this.scheme = scheme;

        // Capture the inner at construction as the value in options will be replaced with
        // a reference to this instance.
        inner = options.TicketDataFormat;
    }

    public string Protect(AuthenticationTicket data) => inner.Protect(data);
    public string Protect(AuthenticationTicket data, string purpose) => inner.Protect(data, purpose);
    public AuthenticationTicket Unprotect(string protectedText) => inner.Unprotect(protectedText);
    public AuthenticationTicket Unprotect(string protectedText, string purpose)
    {
        var ticket = inner.Unprotect(protectedText, purpose);

        if (ticket.Principal.HasClaim(c => c.Type == SessionIdClaim))
        {
            // The ticket is already a reference ticket into the session store, just return it.
            return ticket;
        }

        var context = httpContextAccessor.HttpContext;
        var sessionStore = context.RequestServices.GetRequiredService<IServerSideTicketStore>();

        // Unprotect isn't async so we'll have block the thread and wait using .Result when calling an
        // async methods. This is bad for performance, but this only happens once per logged in user
        // session so it should be acceptable. Thanks to Asp.Net Core not having any synchronization
        // context there is no risk for dead locks.
        var sessionId = sessionStore.StoreAsync(ticket).Result;

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim(SessionIdClaim, sessionId, ClaimValueTypes.String, options.ClaimsIssuer) },
                options.ClaimsIssuer));
        
        ticket = new AuthenticationTicket(principal, null, scheme);

        var cookieValue = inner.Protect(ticket, purpose);

        // Cookie option generation copied from cookie handler.
        var cookieOptions = options.Cookie.Build(context);
        cookieOptions.Expires = null;

        options.CookieManager.AppendResponseCookie(
            context,
            options.Cookie.Name!,
            cookieValue,
            cookieOptions);

        return ticket;
    }
}