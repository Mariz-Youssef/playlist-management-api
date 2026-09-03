using System.ComponentModel.DataAnnotations;

namespace PlaylistManagement.Api.Features.Playlists.DTOs
{
    public class AddSongToPlaylistRequest
    {
        [Required]
        public Guid SongId { get; set; }
    }
}
