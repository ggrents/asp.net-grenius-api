using grenius_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace grenius_api.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(p => p.Username).IsRequired();
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.PasswordHash).IsRequired();

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Username).HasColumnName("username");
            builder.Property(p => p.Email).HasColumnName("email");
            builder.Property(p => p.PasswordHash).HasColumnName("passwordHash");
            builder.Property(p => p.IsActive).HasColumnName("isActive");
        }
    }
}
