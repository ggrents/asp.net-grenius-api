using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Infrastructure.Configurations
{
    public class LyricsConfiguration : IEntityTypeConfiguration<Lyrics>
    {
        public void Configure(EntityTypeBuilder<Lyrics> builder)
        {
            builder.ToTable("lyrics");

            builder.Property(p => p.Text).IsRequired(false);
            builder.Property(p => p.SongId).IsRequired();
            builder.Property(p => p.UserCreatedId).IsRequired();
            
            builder.Property(p => p.Text).HasColumnType("TEXT");

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Text).HasColumnName("text");
            builder.Property(p => p.SongId).HasColumnName("song_id");
            builder.Property(p => p.UserCreatedId).HasColumnName("user_created_id");

             builder.HasOne(p => p.UserCreated)
            .WithMany(c => c.Lyrics)
            .HasForeignKey(p => p.UserCreatedId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
