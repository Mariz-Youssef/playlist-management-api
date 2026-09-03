namespace PlaylistManagement.Api.Features.Authentication.DTOs
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
