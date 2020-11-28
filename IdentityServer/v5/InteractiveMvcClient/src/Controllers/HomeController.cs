using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        [AllowAnonymous]
        public IActionResult Index() => View();

        public IActionResult Secure() => View();

        public IActionResult Logout() => SignOut("oidc");
        
        public async Task<IActionResult> CallApi()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var client = _httpClientFactory.CreateClient();
            client.SetBearerToken(token);

            var response = await client.GetStringAsync(Urls.SampleApi + "identity");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View();
        }
    }
}