using PlaylistManagement.Api.Models.Entities;

namespace PlaylistManagement.Api.Repositories.Interfaces
{
    public interface IPlaylistRepository
    {
        Task AddAsync(Playlist playlist,CancellationToken cancellationToken = default);
        Task<Playlist?> GetByIdAsync(Guid playlistId,CancellationToken cancellationToken = default);
        Task<bool> ContainsSongAsync(Guid playlistId,Guid songId,CancellationToken cancellationToken = default); 
        Task AddSongAsync(PlaylistSong playlistSong,CancellationToken cancellationToken = default);
        Task<List<Playlist>> GetByUserIdAsync(Guid userId,CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        void Remove(Playlist playlist);
        Task<PlaylistSong?> GetPlaylistSongAsync(Guid playlistId,Guid songId,CancellationToken cancellationToken = default);
        void RemoveSong(PlaylistSong playlistSong);

    }
}
