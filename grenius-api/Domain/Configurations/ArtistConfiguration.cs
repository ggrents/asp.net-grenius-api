using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace grenius_api.Domain.Configurations
{
    public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.ToTable("artists");

            builder.Property(p => p.Nickname).IsRequired();

            builder.Property(p => p.Name).HasColumnType("varchar(50)");
            builder.Property(p => p.Surname).HasColumnType("varchar(50)");
            builder.Property(p => p.Nickname).HasColumnType("varchar(50)");


            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Name).HasColumnName("name");
            builder.Property(p => p.Surname).HasColumnName("surname");
            builder.Property(p => p.Nickname).HasColumnName("nickname");
            builder.Property(p => p.Country).HasColumnName("country");
            builder.Property(p => p.Birthday).HasColumnName("birthday");
        }
    }
}
