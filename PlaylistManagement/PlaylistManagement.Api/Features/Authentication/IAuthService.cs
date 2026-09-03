using PlaylistManagement.Api.Features.Authentication.DTOs;

namespace PlaylistManagement.Api.Features.Authentication
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request,CancellationToken cancellationToken = default);
        Task<AuthResponse> LoginAsync(LoginRequest request,CancellationToken cancellationToken = default);
    }
}
