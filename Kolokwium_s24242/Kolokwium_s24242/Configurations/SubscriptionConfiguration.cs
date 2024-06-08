using Kolokwium_s24242.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kolokwium_s24242.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(sub => sub.IdSubscription);
        builder.Property(sub => sub.IdSubscription).ValueGeneratedOnAdd();
        builder.Property(sub => sub.Name).IsRequired().HasMaxLength(100);
        builder.Property(sub => sub.RenewalPeriod).IsRequired();
        builder.Property(sub => sub.EndTime).IsRequired();
        builder.Property(sub => sub.Price).IsRequired().HasColumnType("money");

        builder.HasMany(sub => sub.Sales)
            .WithOne(s => s.Subscription)
            .HasForeignKey(s => s.IdSubscription)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sub => sub.Discounts)
            .WithOne(d => d.Subscription)
            .HasForeignKey(d => d.IdSubscription)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sub => sub.Payments)
            .WithOne(p => p.Subscription)
            .HasForeignKey(p => p.IdSubscription)
            .OnDelete(DeleteBehavior.Cascade);
    }
}