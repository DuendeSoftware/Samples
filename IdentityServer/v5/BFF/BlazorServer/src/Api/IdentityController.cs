using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [Route("identity")]
    public class IdentityController : ControllerBase
    {
        public IActionResult Get()
        {
            var user = User.FindFirst("name")?.Value ?? User.FindFirst("sub").Value;
            return Ok(user);
        }
    }
}