using PlaylistManagement.Api.Features.Playlists.DTOs;

namespace PlaylistManagement.Api.Features.Playlists
{
    public interface IPlaylistService
    {
        Task<PlaylistResponse> CreateAsync(CreatePlaylistRequest request,Guid userId,CancellationToken cancellationToken = default);
        Task AddSongAsync(Guid playlistId,AddSongToPlaylistRequest request, Guid userId,CancellationToken cancellationToken = default);
        Task<List<PlaylistResponse>> GetMyPlaylistsAsync(Guid userId,CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid playlistId, UpdatePlaylistRequest request, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid playlistId, Guid userId, CancellationToken cancellationToken = default);
        Task RemoveSongAsync(Guid playlistId, Guid songId, Guid userId, CancellationToken cancellationToken = default);

    }
}
