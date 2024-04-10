using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Domain.Configurations
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("albums");

            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.ArtistId).IsRequired();
            builder.Property(p => p.AlbumTypeId).IsRequired();
            builder.Property(p => p.ReleaseDate).IsRequired();

            builder.Property(p => p.Title).HasColumnType("varchar(50)");

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Title).HasColumnName("title");
            builder.Property(p => p.ReleaseDate).HasColumnName("releaseDate");
            builder.Property(p => p.ArtistId).HasColumnName("artist_id");
            builder.Property(p => p.AlbumTypeId).HasColumnName("album_type_id");
        }
    }
}
