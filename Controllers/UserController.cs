using Microsoft.AspNetCore.Mvc;
using persist_net_backend.DTOs;
using persist_net_backend.Services;

namespace persist_net_backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (success, token) = await _authService.LoginAsync(request.Email, request.Password);
            if (!success)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var success = await _authService.RegisterAsync(request.Name, request.Surname, request.Email, request.Password );
            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}