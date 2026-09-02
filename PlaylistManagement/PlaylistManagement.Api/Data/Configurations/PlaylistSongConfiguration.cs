using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaylistManagement.Api.Models.Entities;

namespace PlaylistManagement.Api.Data.Configurations
{
    public class PlaylistSongConfiguration : IEntityTypeConfiguration<PlaylistSong>
    {
        public void Configure(EntityTypeBuilder<PlaylistSong> builder)
        {
            builder.HasKey(ps => new
            {
                ps.PlaylistId,
                ps.SongId
            });

            builder.Property(ps => ps.AddedAt).IsRequired();
        }
    }
}
