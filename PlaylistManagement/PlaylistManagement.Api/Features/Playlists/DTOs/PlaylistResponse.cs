namespace PlaylistManagement.Api.Features.Playlists.DTOs
{
    public class PlaylistResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public List<SongResponse> Songs { get; set; } = [];
    }
}
