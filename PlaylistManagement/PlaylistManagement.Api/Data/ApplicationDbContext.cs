using Microsoft.EntityFrameworkCore;
using PlaylistManagement.Api.Models.Entities;

namespace PlaylistManagement.Api.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<User> Users {  get; set; }
        public DbSet<Playlist> Playlists {  get; set; }
        public DbSet<Song> Songs {  get; set; }
        public DbSet<PlaylistSong> PlaylistSongs {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
