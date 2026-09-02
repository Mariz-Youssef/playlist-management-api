namespace PlaylistManagement.Api.Models.Entities
{
    //conjunction table
    public class PlaylistSong{
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
        public DateTime AddedAt { get; set; }
        public Playlist Playlist { get; set; } = null!;
        public Song Song { get; set; } = null!;
    }
}
