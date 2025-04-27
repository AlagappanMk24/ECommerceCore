using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Infrastructure.Data.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // Invoice - Customer relationship
            builder.HasOne(i => i.Customer)
                   .WithMany(c => c.Invoices)
                   .HasForeignKey(i => i.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a Customer if Invoices exist

            // Invoice - Company relationship
            builder.HasOne(i => i.Company)
                   .WithMany()
                   .HasForeignKey(i => i.CompanyId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a Company if Invoices exist

            // Invoice - Location relationship
            builder.HasOne(i => i.Location)
                   .WithMany(l => l.Invoices)
                   .HasForeignKey(i => i.LocationId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a Location if Invoices exist

            // Invoice - OrderHeader relationship (optional)
            builder.HasOne(i => i.Order)
                   .WithMany()
                   .HasForeignKey(i => i.OrderId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false); // OrderId is nullable

            // Invoice - InvoiceItems relationship
            builder.HasMany(i => i.InvoiceItems)
                   .WithOne(ii => ii.Invoice)
                   .HasForeignKey(ii => ii.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete InvoiceItems when Invoice is deleted

            // Invoice - InvoiceAttachments relationship
            builder.HasMany(i => i.InvoiceAttachments)
                   .WithOne(ia => ia.Invoice)
                   .HasForeignKey(ia => ia.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete InvoiceAttachments when Invoice is deleted

            // Invoice - TaxDetails relationship
            builder.HasMany(i => i.TaxDetails)
                   .WithOne(td => td.Invoice)
                   .HasForeignKey(td => td.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete TaxDetails when Invoice is deleted

            // Ensure InvoiceNumber is unique
            builder.HasIndex(i => i.InvoiceNumber)
                   .IsUnique();

            // Configure decimal properties for currency fields
            builder.Property(i => i.Subtotal).HasColumnType("decimal(18,2)");
            builder.Property(i => i.Discount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.Tax).HasColumnType("decimal(18,2)");
            builder.Property(i => i.ShippingAmount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.PaidAmount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
        }
    }
}

#region
//Invoice:
//Customer: One - to - many(Invoice belongs to one Customer, a Customer can have many Invoices).
//Company: One - to - many(Invoice belongs to one Company, a Company can have many Invoices).
//Location: One - to - many(Invoice belongs to one Location, a Location can have many Invoices).
//OrderHeader: One - to - many(optional, Invoice may relate to one OrderHeader, an OrderHeader can have multiple Invoices).
//InvoiceItems: One - to - many(Invoice has many InvoiceItems, an InvoiceItem belongs to one Invoice).
//InvoiceAttachments: One - to - many(Invoice has many InvoiceAttachments, an InvoiceAttachments belongs to one Invoice).
//TaxDetails: One - to - many(Invoice has many TaxDetails, a TaxDetail belongs to one Invoice).
#endregion