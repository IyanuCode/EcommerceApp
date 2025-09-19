using Ecommerce.Api.DTOs.AuthDto;
using Microsoft.AspNetCore.Mvc;
namespace Ecommerce.Api.Controllers
{
    [ApiController] //This attribute makes the controller behave as a web API controller
    [Route("api/[controller]")] //This attribute defines the routing pattern for the controller
    public class AuthController : ControllerBase
    {
        [HttpPost("login")] // This attribute says: this endpoint responds to HTTP POST requests at "api/Auth/login".
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (request.Username == "admin" && request.Password == "123")
            {
                var token = "GeneratedJWTHere";
                return Ok(new { Token = token });

            } // If the request JSON contains username = "admin" and password = "123", then login succeeds. otherwise
            return Unauthorized("Invalid credentials");
        }
    }
}
