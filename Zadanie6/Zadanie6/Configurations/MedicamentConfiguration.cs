using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zadanie6.Models;

namespace Zadanie6.Configurations;

public class MedicamentConfiguration : IEntityTypeConfiguration<Medicament>
{
    public void Configure(EntityTypeBuilder<Medicament> builder)
    {
        builder.HasKey(d => d.IdMedicament);
        builder.Property(d => d.IdMedicament).ValueGeneratedOnAdd();    
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Description).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Type).IsRequired().HasMaxLength(100);
  
    }
}