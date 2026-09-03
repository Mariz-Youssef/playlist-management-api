using PlaylistManagement.Api.Features.Authentication.DTOs;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Interfaces;
using BCrypt.Net;
using PlaylistManagement.Api.Exceptions;


namespace PlaylistManagement.Api.Features.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository,ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email,cancellationToken);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password"); 
            }

            return _tokenService.GenerateAccessToken(user);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var emailExists = await _userRepository.EmailExistsAsync(email,cancellationToken);
            if (emailExists)
            {
                throw new ConflictException("A user with this email already exists"); 
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };
            await _userRepository.AddAsync(user, cancellationToken);
            return _tokenService.GenerateAccessToken(user);
        }
    }
}
