using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Infrastructure.Configurations
{
    public class ArtistRatingConfiguration : IEntityTypeConfiguration<ArtistRating>
    {
        public void Configure(EntityTypeBuilder<ArtistRating> builder)
        {
            builder.ToTable("artists_rating");

            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Count).HasColumnType("BIGINT");
            
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Count).HasColumnName("count");
            builder.Property(p => p.ArtistId).HasColumnName("artist_id");
        }
    }
}
