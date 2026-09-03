using PlaylistManagement.Api.Features.Authentication.DTOs;
using PlaylistManagement.Api.Models.Entities;

namespace PlaylistManagement.Api.Features.Authentication
{
    public interface ITokenService
    {
        //to return the token and its expiration time together
        AuthResponse GenerateAccessToken(User user);
    }
}
