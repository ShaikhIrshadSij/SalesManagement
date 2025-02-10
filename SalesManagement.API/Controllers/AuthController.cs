using Microsoft.AspNetCore.Mvc;
using SalesManagement.API.Interfaces;
using SalesManagement.API.Models;

namespace SalesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _authService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
                return Unauthorized();

            var token = await _authService.GenerateJwtTokenAsync(user);

            return Ok(new
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}
