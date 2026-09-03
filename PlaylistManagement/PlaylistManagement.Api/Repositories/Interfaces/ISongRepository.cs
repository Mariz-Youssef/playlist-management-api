using PlaylistManagement.Api.Models.Entities;

namespace PlaylistManagement.Api.Repositories.Interfaces
{
    public interface ISongRepository
    {
        Task<Song?> GetByIdAsync(Guid songId,CancellationToken cancellationToken = default);
    }
}
