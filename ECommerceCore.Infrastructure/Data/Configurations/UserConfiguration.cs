using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // ApplicationUser - Company relationship
            builder.HasOne(au => au.Company)
                .WithMany()
                .HasForeignKey(au => au.CompanyId)
                .IsRequired(false);
        }
    }
}