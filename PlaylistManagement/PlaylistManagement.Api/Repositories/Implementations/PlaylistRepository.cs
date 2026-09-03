using Microsoft.EntityFrameworkCore;
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

        public async Task AddSongAsync(PlaylistSong playlistSong, CancellationToken cancellationToken = default)
        {
            await _context.PlaylistSongs.AddAsync(playlistSong, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ContainsSongAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken = default)
        {
            return await _context.PlaylistSongs.AnyAsync(ps => ps.PlaylistId == playlistId &&ps.SongId == songId,cancellationToken);
        }

        public async Task<Playlist?> GetByIdAsync(Guid playlistId, CancellationToken cancellationToken = default)
        {
            return await _context.Playlists.FirstOrDefaultAsync(p => p.Id == playlistId,cancellationToken);
        }

        public async Task<List<Playlist>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Playlists
             .AsNoTracking()
             .Where(p => p.UserId == userId)
             .Include(p => p.PlaylistSongs)
                 .ThenInclude(ps => ps.Song)
             .OrderBy(p => p.Name)
             .ToListAsync(cancellationToken);
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public void Remove(Playlist playlist)
        {
            _context.Playlists.Remove(playlist);
        }
    }
}
