using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [Route("identity")]
    public class IdentityController : ControllerBase
    {
        public IActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}