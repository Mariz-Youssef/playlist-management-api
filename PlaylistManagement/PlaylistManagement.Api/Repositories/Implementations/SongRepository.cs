using Microsoft.EntityFrameworkCore;
using PlaylistManagement.Api.Data;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Interfaces;

namespace PlaylistManagement.Api.Repositories.Implementations
{
    public class SongRepository : ISongRepository
    {
        private readonly ApplicationDbContext _context;
        public SongRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Song?> GetByIdAsync(Guid songId, CancellationToken cancellationToken = default)
        {
            return await _context.Songs.FirstOrDefaultAsync(s => s.Id == songId,cancellationToken);
        }
    }
}
