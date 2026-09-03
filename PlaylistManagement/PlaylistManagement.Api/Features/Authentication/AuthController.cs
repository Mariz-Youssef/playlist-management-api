using Microsoft.AspNetCore.Mvc;
using PlaylistManagement.Api.Features.Authentication.DTOs;

namespace PlaylistManagement.Api.Features.Authentication
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request,CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterAsync(request,cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request,CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(request,cancellationToken);
            return Ok(response);
        }
    }
}
