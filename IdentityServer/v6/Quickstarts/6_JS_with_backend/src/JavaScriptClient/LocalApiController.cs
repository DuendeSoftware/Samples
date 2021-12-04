// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaScriptClient
{
    public class LocalApiController : ControllerBase
    {
        [Route("local/identity")]
        [Authorize]
        public IActionResult Get()
        {
            //var token = await HttpContext.GetUserAccessTokenAsync();

            var name = User.FindFirst("name")?.Value ?? User.FindFirst("sub")?.Value;
            return new JsonResult(new { message = "Local API Success!", user=name });
        }
    }
}
