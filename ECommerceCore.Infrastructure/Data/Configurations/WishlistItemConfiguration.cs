using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            // WishlistItems relationship
            builder.HasOne(w => w.Product)
                    .WithMany()
                    .HasForeignKey(w => w.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.User)
                    .WithMany()
                    .HasForeignKey(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}