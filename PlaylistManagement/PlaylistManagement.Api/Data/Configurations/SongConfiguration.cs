using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaylistManagement.Api.Models.Entities;

namespace PlaylistManagement.Api.Data.Configurations
{
    public class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Artist)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(s => s.PlaylistSongs)
                .WithOne(ps => ps.Song)
                .HasForeignKey(ps => ps.SongId)
                .OnDelete(DeleteBehavior.Restrict); //a song that has a ref in a playlist cannot be deleted

            //using AI
            builder.HasData(
            new Song
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Bohemian Rhapsody",
                Artist = "Queen"
            },
            new Song
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Billie Jean",
                Artist = "Michael Jackson"
            },
            new Song
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Hotel California",
                Artist = "Eagles"
            },
            new Song
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Title = "Shape of You",
                Artist = "Ed Sheeran"
            },
            new Song
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Title = "Smells Like Teen Spirit",
                Artist = "Nirvana"
            }
        );

        }

    }
}
