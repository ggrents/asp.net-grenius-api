using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace grenius_api.Infrastructure.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("genres");

            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.Name).HasColumnType("varchar(50)");
            builder.Property(p => p.Description).HasColumnType("TEXT");

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Name).HasColumnName("name");
            builder.Property(p => p.Description).HasColumnName("description");
        }
    }
}
