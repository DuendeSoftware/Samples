using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client
{
    public class LogoutModel : PageModel
    {
        public SignOutResult OnGet()
        {
            return SignOut("cookie", "oidc");
        }
    }
}
