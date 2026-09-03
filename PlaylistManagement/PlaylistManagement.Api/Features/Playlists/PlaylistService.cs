using PlaylistManagement.Api.Features.Playlists.DTOs;
using PlaylistManagement.Api.Features.Songs.DTOs;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Implementations;
using PlaylistManagement.Api.Repositories.Interfaces;

namespace PlaylistManagement.Api.Features.Playlists
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;

        public PlaylistService(IPlaylistRepository playlistRepository,ISongRepository songRepository)
        {
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
        }

        public async Task AddSongAsync(Guid playlistId, AddSongToPlaylistRequest request, Guid userId, CancellationToken cancellationToken = default)
        {
            var playlist = await _playlistRepository.GetByIdAsync(playlistId,cancellationToken);

            if (playlist is null)
            {
                throw new KeyNotFoundException("Playlist not found");
            }
            if (playlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("you do not have access to this playlist");
            }
            var song = await _songRepository.GetByIdAsync(request.SongId,cancellationToken);

            if (song is null)
            {
                throw new KeyNotFoundException("song not found");
            }
            var alreadyExists = await _playlistRepository.ContainsSongAsync(playlistId,request.SongId,cancellationToken);
            if (alreadyExists)
            {
                throw new InvalidOperationException("The song is already in this playlist");
            }
            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = request.SongId,
                AddedAt = DateTime.UtcNow
            };

            await _playlistRepository.AddSongAsync(playlistSong,cancellationToken);
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

        public async Task<List<PlaylistResponse>> GetMyPlaylistsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var playlists = await _playlistRepository.GetByUserIdAsync(userId,cancellationToken);
            return playlists
               .Select(p => new PlaylistResponse
               {
                   Id = p.Id,
                   Name = p.Name,
                   CreatedAt = p.CreatedAt,
                   Songs = p.PlaylistSongs
                       .Select(ps => new SongResponse
                       {
                           Id = ps.Song.Id,
                           Title = ps.Song.Title,
                           Artist = ps.Song.Artist
                       })
                       .ToList()
               })
               .ToList();
        }
        public async Task UpdateAsync(Guid playlistId,UpdatePlaylistRequest request,Guid userId,CancellationToken cancellationToken = default)
        {
            var playlist = await _playlistRepository.GetByIdAsync(playlistId,cancellationToken);

            if (playlist is null)
            {
                throw new KeyNotFoundException("playlist not found");
            }

            if (playlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have access to this playlist");
            }

            playlist.Name = request.Name.Trim();

            await _playlistRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Guid playlistId,Guid userId,CancellationToken cancellationToken = default)
        {
            var playlist = await _playlistRepository.GetByIdAsync(playlistId,cancellationToken);

            if (playlist is null)
            {
                throw new KeyNotFoundException("Playlist not found");
            }

            if (playlist.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this playlist");
            }

            _playlistRepository.Remove(playlist);

            await _playlistRepository.SaveChangesAsync(cancellationToken);
        }
        public async Task RemoveSongAsync(Guid playlistId,Guid songId,Guid userId,CancellationToken cancellationToken = default)
        {
            var playlist = await _playlistRepository.GetByIdAsync(playlistId,cancellationToken);

            if (playlist is null)
            {
                throw new KeyNotFoundException("Playlist not found");
            }

            if (playlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have access to this playlist.");
            }

            var playlistSong = await _playlistRepository.GetPlaylistSongAsync(playlistId,songId,cancellationToken);

            if (playlistSong is null)
            {
                throw new KeyNotFoundException("Song is not in this playlist.");
            }

            _playlistRepository.RemoveSong(playlistSong);

            await _playlistRepository.SaveChangesAsync(
                cancellationToken);
        }
    }
}
