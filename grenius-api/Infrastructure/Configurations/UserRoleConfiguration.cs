using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using grenius_api.Domain.Entities;

namespace grenius_api.Infrastructure.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("users_roles");

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.UserId).HasColumnName("user_id");
            builder.Property(p => p.RoleId).HasColumnName("role_id");
        }
    }
}
