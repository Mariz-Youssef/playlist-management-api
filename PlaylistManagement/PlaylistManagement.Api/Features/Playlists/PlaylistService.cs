using PlaylistManagement.Api.Features.Playlists.DTOs;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Interfaces;

namespace PlaylistManagement.Api.Features.Playlists
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        public PlaylistService(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        public async Task<PlaylistResponse> CreateAsync(CreatePlaylistRequest request, Guid userId, CancellationToken cancellationToken = default)
        {
            var playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _playlistRepository.AddAsync(playlist,cancellationToken);
            return new PlaylistResponse
            {
                Id = playlist.Id,
                Name = playlist.Name,
                CreatedAt = playlist.CreatedAt
            };
        }
    }
}
