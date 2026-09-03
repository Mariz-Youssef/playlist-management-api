using PlaylistManagement.Api.Features.Playlists.DTOs;

namespace PlaylistManagement.Api.Features.Playlists
{
    public interface IPlaylistService
    {
        Task<PlaylistResponse> CreateAsync(CreatePlaylistRequest request,Guid userId,CancellationToken cancellationToken = default);
        Task AddSongAsync(Guid playlistId,AddSongToPlaylistRequest request, Guid userId,CancellationToken cancellationToken = default);
        Task<List<PlaylistResponse>> GetMyPlaylistsAsync(Guid userId,CancellationToken cancellationToken = default);
    }
}
