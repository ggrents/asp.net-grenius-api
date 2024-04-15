using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace grenius_api.Infrastructure.Configurations
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.ToTable("producers");

            builder.Property(p => p.Nickname).IsRequired();

            builder.Property(p => p.Name).HasColumnType("varchar(50)");
            builder.Property(p => p.Surname).HasColumnType("varchar(50)");
            builder.Property(p => p.Nickname).HasColumnType("varchar(50)");

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Name).HasColumnName("name");
            builder.Property(p => p.Surname).HasColumnName("surname");
            builder.Property(p => p.Nickname).HasColumnName("nickname");
            builder.Property(p => p.Country).HasColumnName("country");
        }
    }
}

