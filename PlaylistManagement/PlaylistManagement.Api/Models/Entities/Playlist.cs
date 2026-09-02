namespace PlaylistManagement.Api.Models.Entities
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = null!;
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

    }
}
