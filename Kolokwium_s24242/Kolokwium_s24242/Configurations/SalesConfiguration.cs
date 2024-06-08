using Kolokwium_s24242.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kolokwium_s24242.Configurations;

public class SalesConfiguration: IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.IdSale);
        builder.Property(s => s.IdSale).ValueGeneratedOnAdd();
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasOne(s => s.Client)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.IdClient)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Subscription)
            .WithMany(sub => sub.Sales)
            .HasForeignKey(s => s.IdSubscription)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
