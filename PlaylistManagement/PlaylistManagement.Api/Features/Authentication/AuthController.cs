using Microsoft.AspNetCore.Mvc;
using PlaylistManagement.Api.Features.Authentication.DTOs;

namespace PlaylistManagement.Api.Features.Authentication
{
    /// <summary>
    /// Provides endpoints for user registration and authentication.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Registers a new user and returns an access token.
        /// </summary>
        /// <param name="request">
        /// The user's registration information.
        /// </param>
        /// <param name="cancellationToken">
        /// Token used to cancel the request.
        /// </param>
        /// <returns>
        /// A JWT access token and its expiration time.
        /// </returns>
        /// <response code="200">
        /// User registered successfully.
        /// </response>
        /// <response code="409">
        /// A user with the specified email already exists.
        /// </response>
        /// <response code="400">
        /// The request data is invalid.
        /// </response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request,CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterAsync(request,cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT access token.
        /// </summary>
        /// <param name="request">
        /// The user's login credentials.
        /// </param>
        /// <param name="cancellationToken">
        /// Token used to cancel the request.
        /// </param>
        /// <returns>
        /// A JWT access token and its expiration time.
        /// </returns>
        /// <response code="200">
        /// Authentication succeeded.
        /// </response>
        /// <response code="401">
        /// The email or password is invalid.
        /// </response>
        /// <response code="400">
        /// The request data is invalid.
        /// </response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request,CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(request,cancellationToken);
            return Ok(response);
        }
    }
}
