using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Company - Invoices relationship
            builder.HasMany(c => c.Invoices)
                   .WithOne(i => i.Company)
                   .HasForeignKey(i => i.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Company - Locations relationship
            builder.HasMany(c => c.Locations)
                   .WithOne(l => l.Company)
                   .HasForeignKey(l => l.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
