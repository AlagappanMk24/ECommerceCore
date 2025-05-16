using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {      
        // Define many-to-many relationship between roles and permissions
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(rp => rp.RoleId);

            builder.HasOne(rp => rp.Permission)
           .WithMany(p => p.RolePermissions)
           .HasForeignKey(rp => rp.PermissionId);
        }
    }
}
