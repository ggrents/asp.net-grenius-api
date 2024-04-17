using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Infrastructure.Configurations
{
    public class AnnotationConfiguration : IEntityTypeConfiguration<Annotation>
    {
        public void Configure(EntityTypeBuilder<Annotation> builder)
        {
            builder.ToTable("annotation");

            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Text).IsRequired();
            builder.Property(p => p.StartSymbol).IsRequired();
            builder.Property(p => p.EndSymbol).IsRequired();
            builder.Property(p => p.UserCreatedId).IsRequired();

            builder.Property(p => p.Text).HasColumnType("TEXT");

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Text).HasColumnName("text");
            builder.Property(p => p.StartSymbol).HasColumnName("start_symbol");
            builder.Property(p => p.EndSymbol).HasColumnName("end_symbol");
            builder.Property(p => p.LyricsId).HasColumnName("lyrics_id");
            builder.Property(p => p.UserCreatedId).HasColumnName("user_created_id");
        }
    }
}
