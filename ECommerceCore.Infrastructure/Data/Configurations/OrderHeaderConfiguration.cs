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

            // OrderHeader - Customer relationship
            builder.HasOne(oh => oh.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(oh => oh.CustomerId)
                    .IsRequired(false) // If order can exist without customer
                    .OnDelete(DeleteBehavior.NoAction);

            // OrderHeader - Invoices relationship
            builder.HasMany(oh => oh.Invoices)
                   .WithOne(i => i.Order)
                   .HasForeignKey(i => i.OrderId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Additional configurations
            builder.Property(oh => oh.OrderTotal)
                   .HasColumnType("decimal(18,2)");

            builder.Property(oh => oh.OrderStatus)
                   .HasMaxLength(50);

            builder.Property(oh => oh.PaymentStatus)
                   .HasMaxLength(50);

            // Configure Shipping Address as an owned type
            builder.OwnsOne(l => l.ShipToAddress, shippingAddress =>
            {
                shippingAddress.Property(a => a.ShippingAddress1).HasColumnName("ShippingAddress1").IsRequired();
                shippingAddress.Property(a => a.ShippingAddress2).HasColumnName("ShippingAddress2").IsRequired(false);
                shippingAddress.Property(a => a.ShippingCity).HasColumnName("ShippingCity").IsRequired();
                shippingAddress.Property(a => a.ShippingState).HasColumnName("ShippingState").IsRequired(false);
                shippingAddress.Property(a => a.ShippingCountry).HasColumnName("ShippingCountry").IsRequired();
                shippingAddress.Property(a => a.ShippingZipCode).HasColumnName("ShippingZipCode").IsRequired();
            });
        }
    }
}
   