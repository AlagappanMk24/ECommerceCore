using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class TimezoneConfiguration : IEntityTypeConfiguration<Timezone>
    {
        public void Configure(EntityTypeBuilder<Timezone> builder)
        {
            // Ensure Name is unique
            builder.HasIndex(t => t.Name)
                   .IsUnique();

            // Timezone - Locations relationship (optional, if you want to define the inverse)
            builder.HasMany<Location>()
                   .WithOne(l => l.Timezone)
                   .HasForeignKey(l => l.TimezoneId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

#region
// Locations: One-to-many (a Timezone can be used by many Locations).
#endregion