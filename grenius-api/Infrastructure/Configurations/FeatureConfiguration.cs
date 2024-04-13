using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace grenius_api.Infrastructure.Configurations
{
    public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> builder)
        {
            builder.ToTable("features");

            builder.Property(p => p.Priority).IsRequired();
            builder.Property(p => p.SongId).IsRequired();
            builder.Property(p => p.ArtistId).IsRequired();

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Priority).HasColumnName("priority");
            builder.Property(p => p.SongId).HasColumnName("song_id");
            builder.Property(p => p.ArtistId).HasColumnName("artist_id");

            builder.HasOne(p => p.Song)
                    .WithMany(c => c.Features)
                    .HasForeignKey(p => p.SongId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Artist)
                    .WithMany(c => c.Features)
                    .HasForeignKey(p => p.ArtistId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
