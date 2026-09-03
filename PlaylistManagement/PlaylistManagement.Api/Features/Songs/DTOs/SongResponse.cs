namespace PlaylistManagement.Api.Features.Songs.DTOs
{
    public class SongResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
    }
}
