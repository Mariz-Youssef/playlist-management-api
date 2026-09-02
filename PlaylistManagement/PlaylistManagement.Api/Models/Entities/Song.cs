namespace PlaylistManagement.Api.Models.Entities
{
    public class Song
    {
        public Guid Id { get; set; }

        public string Title { get; set; } ="";

        public string Artist { get; set; } ="";

        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
