using Kolokwium_s24242.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kolokwium_s24242.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.IdDiscount);
            builder.Property(d => d.IdDiscount).ValueGeneratedOnAdd();
            builder.Property(d => d.Value).IsRequired();
            builder.Property(d => d.DateFrom).IsRequired();
            builder.Property(d => d.DateTo).IsRequired();

            builder.HasOne(d => d.Subscription)
                .WithMany(sub => sub.Discounts)
                .HasForeignKey(d => d.IdSubscription)
                .OnDelete(DeleteBehavior.Cascade);
        }
}
