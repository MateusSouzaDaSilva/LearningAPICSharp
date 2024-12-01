using LearningWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningWebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == "mateus" && password == "12345")
            {
                var token = TokenService.GenerateToken(new Model.Employee());
                return Ok("Bearer " + token.ToString());
            }
            return BadRequest("Username or password invalid");
        }
    }
}
