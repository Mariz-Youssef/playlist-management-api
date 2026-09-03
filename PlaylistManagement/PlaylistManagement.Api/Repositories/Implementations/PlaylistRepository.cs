using PlaylistManagement.Api.Data;
using PlaylistManagement.Api.Features.Playlists;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Interfaces;

namespace PlaylistManagement.Api.Repositories.Implementations
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _context;
        public PlaylistRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Playlist playlist, CancellationToken cancellationToken = default)
        {
            await _context.Playlists.AddAsync(playlist, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
