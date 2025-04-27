using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            // Ensure Code is unique
            builder.HasIndex(c => c.Code)
                   .IsUnique();

            // Currency - Locations relationship (optional, if you want to define the inverse)
            builder.HasMany<Location>()
                   .WithOne(l => l.Currency)
                   .HasForeignKey(l => l.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

#region
// Locations: One-to-many (a Currency can be used by many Locations).
#endregion