namespace PlaylistManagement.Api.Models.Entities
{
    public class User
    {
        public Guid Id {  get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        //1:M relationship
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();


    }
}
