using System.ComponentModel.DataAnnotations;

namespace PlaylistManagement.Api.Features.Playlists.DTOs
{
    public class CreatePlaylistRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } ="";

    }
}
