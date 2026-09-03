using PlaylistManagement.Api.Models.Entities;

namespace PlaylistManagement.Api.Repositories.Interfaces
{
    public interface IPlaylistRepository
    {
       Task AddAsync(Playlist playlist,CancellationToken cancellationToken = default);
    }
}
