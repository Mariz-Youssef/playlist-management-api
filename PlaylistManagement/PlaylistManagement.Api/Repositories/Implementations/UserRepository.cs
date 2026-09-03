using Microsoft.EntityFrameworkCore;
using PlaylistManagement.Api.Data;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Interfaces;

namespace PlaylistManagement.Api.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) { 
            _context = context;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(u=>u.Email==email,cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
    }
}
