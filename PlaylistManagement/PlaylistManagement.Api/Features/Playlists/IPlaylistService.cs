using PlaylistManagement.Api.Features.Playlists.DTOs;

namespace PlaylistManagement.Api.Features.Playlists
{
    public interface IPlaylistService
    {
        Task<PlaylistResponse> CreateAsync(CreatePlaylistRequest request,Guid userId,CancellationToken cancellationToken = default);
    }
}
