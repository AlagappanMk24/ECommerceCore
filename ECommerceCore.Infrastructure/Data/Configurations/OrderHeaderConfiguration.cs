using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class OrderHeaderConfiguration : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            // OrderHeader - ApplicationUser relationship
            builder.HasOne(oh => oh.ApplicationUser)
                    .WithMany()
                    .HasForeignKey(oh => oh.ApplicationUserId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
   