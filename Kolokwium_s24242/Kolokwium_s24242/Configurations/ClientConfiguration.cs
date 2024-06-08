using Kolokwium_s24242.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kolokwium_s24242.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(d => d.IdClient);
            builder.Property(d => d.IdClient).ValueGeneratedOnAdd();
            builder.Property(d => d.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(d => d.LastName).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Email).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Phone).IsRequired(false).HasMaxLength(100);
            
            builder.HasMany(c => c.Sales)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.IdClient)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Payments)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.IdClient)
                .OnDelete(DeleteBehavior.Cascade);

        }
}