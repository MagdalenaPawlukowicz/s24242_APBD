using Microsoft.EntityFrameworkCore;
using Zadanie6.Models;

namespace Zadanie6.Zadanie6Context;

public class Zadanie6Context: DbContext
{
    public Zadanie6Context() { }

    public Zadanie6Context(DbContextOptions<Zadanie6Context> options)
        : base(options) { }
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        
    }
}