﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ECommerceCore.Domain.Entities.Identity;

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

            // Optimize the query with an index on IsDeleted and DeletedDate
            builder.HasIndex(u => new { u.IsDeleted, u.DeletedDate })
            .HasFilter("IsDeleted = 1")
            .HasDatabaseName("IX_AspNetUsers_IsDeleted_DeletedDate");
        }
    }
}