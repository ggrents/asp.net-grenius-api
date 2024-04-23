using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Infrastructure.Configurations
{
    public class SongRatingConfiguration : IEntityTypeConfiguration<SongRating>
    {
        public void Configure(EntityTypeBuilder<SongRating> builder)
        {
            builder.ToTable("songs_rating");

            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Count).HasColumnType("BIGINT");

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Count).HasColumnName("count");
            builder.Property(p => p.SongId).HasColumnName("song_id");
        }
    }
}
